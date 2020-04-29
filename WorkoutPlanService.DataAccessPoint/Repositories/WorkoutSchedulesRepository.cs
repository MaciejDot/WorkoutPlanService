using CacheManager.Core;
using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DatetimeService;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public class WorkoutSchedulesRepository : IWorkoutSchedulesRepository
    {
        private readonly IWorkoutScheduleCacheService _workoutScheduleCacheService;
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IDateTimeService _dateTimeService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public WorkoutSchedulesRepository(
            IWorkoutScheduleCacheService workoutScheduleCacheService,
            IWorkoutPlanRepository workoutPlanRepository,
            IQueryProcessor queryProcessor,
            IDateTimeService dateTimeService,
            IBackgroundJobClientService backgroundJobClientService)
        {
            _workoutScheduleCacheService = workoutScheduleCacheService;
            _workoutPlanRepository = workoutPlanRepository;
            _queryProcessor = queryProcessor;
            _dateTimeService = dateTimeService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public Task<IEnumerable<WorkoutScheduleDTO>> GetWorkoutSchedules(string username)
        {
            var schedules = _workoutScheduleCacheService.Get(username);
            return HandleNullCacheItem(username, schedules);
        }

        private async Task<IEnumerable<WorkoutScheduleDTO>> HandleNullCacheItem(string username, CacheItem<IEnumerable<WorkoutScheduleDTO>> cacheItem) 
        {
            if (cacheItem == null)
            {
                var schedulesFromDatabase = await _queryProcessor.Process(new GetUserWorkoutSchedulesQuery { Username = username}, default);
                _workoutScheduleCacheService.Put(username, schedulesFromDatabase);
                return schedulesFromDatabase;
            }
            else 
            {
                return cacheItem.Value;
            }
        }

        public async Task<Guid> AddWorkoutScheduleAsync(string username, WorkoutScheduleDTO workoutScheduleDTO)
        {
            await ValidateWorkoutPlanSchedule(username, workoutScheduleDTO);
            var schedules = await GetWorkoutSchedules(username);
            if (ValidateWorkoutScheduleExist(schedules, workoutScheduleDTO)) 
            {
                throw new Exception("That Workout Already Exist");
            }
            var list = schedules.ToList();
            list.Add(workoutScheduleDTO);
            _workoutScheduleCacheService.Put(username, list);
            _backgroundJobClientService.Enqueue<IAddWorkoutScheduleJob>(x => x.Run(new AddWorkoutScheduleCommand 
            { 
                ExternalId = workoutScheduleDTO.ExternalId,
                Created = _dateTimeService.GetCurrentDate(),
                FirstDate = workoutScheduleDTO.FirstDate,
                WorkoutPlanExternalId = workoutScheduleDTO.WorkoutPlanExternalId,
                Recurrence = workoutScheduleDTO.Recurrence,
                RecurringTimes = workoutScheduleDTO.RecurringTimes
            }));
            return workoutScheduleDTO.ExternalId;
        }

        public async Task DeleteWorkoutScheduleAsync(string username, Guid externalId)
        {
            var schedules = await GetWorkoutSchedules(username);
            if (!schedules.Any(x=>x.ExternalId ==externalId))
            {
                throw new Exception("That Workout dont Exist");
            }
            var workoutPlanId = schedules.First().WorkoutPlanExternalId;
            var list = schedules.Where(x => x.ExternalId != externalId).ToList();

            _workoutScheduleCacheService.Put(username, list);
            _backgroundJobClientService.Enqueue<IDeleteWorkoutScheduleJob>(x => x.Run(new DeleteWorkoutScheduleCommand
            {
                ExternalId =externalId,
                Created = _dateTimeService.GetCurrentDate(),
                WorkoutPlanExternalId = workoutPlanId
            }));
        }

        public async Task UpdateWorkoutScheduleAsync(string username, WorkoutScheduleDTO workoutScheduleDTO)
        {
            await ValidateWorkoutPlanSchedule(username, workoutScheduleDTO);
            var schedules = await GetWorkoutSchedules(username);
            if (!ValidateWorkoutScheduleExist(schedules, workoutScheduleDTO))
            {
                throw new Exception("That Workout dont Exist");
            }
            var list = schedules.Where(x=>x.ExternalId != workoutScheduleDTO.ExternalId).ToList();
            list.Add(workoutScheduleDTO);
            _workoutScheduleCacheService.Put(username, list);
            _backgroundJobClientService.Enqueue<IAddWorkoutScheduleJob>(x => x.Run(new AddWorkoutScheduleCommand
            {
                ExternalId = workoutScheduleDTO.ExternalId,
                Created = _dateTimeService.GetCurrentDate(),
                FirstDate = workoutScheduleDTO.FirstDate,
                WorkoutPlanExternalId = workoutScheduleDTO.WorkoutPlanExternalId,
                Recurrence = workoutScheduleDTO.Recurrence,
                RecurringTimes = workoutScheduleDTO.RecurringTimes
            }));
        }

        private bool ValidateWorkoutScheduleExist(IEnumerable<WorkoutScheduleDTO> schedules, WorkoutScheduleDTO workoutScheduleDTO)
        {
            return schedules.Any(x => x.ExternalId == workoutScheduleDTO.ExternalId);
        }

        private async Task ValidateWorkoutPlanSchedule(string username, WorkoutScheduleDTO workoutScheduleDTO) 
        {
            var plans = await _workoutPlanRepository.GetAllUserWorkutPlansAsync(username);
            if (! plans.Any(x=>x.ExternalId == workoutScheduleDTO.WorkoutPlanExternalId))
            {
                throw new Exception("There is no workout with given externalId");
            }
        }
    }
}
