using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;
using SimpleCQRS.Query;
using WorkoutPlanService.DataAccessPoint.Database.Query;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public sealed class WorkoutPlanRepository : IWorkoutPlanRepository
    {
        private readonly IWorkoutPlanCacheService _workoutPlanCacheService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public WorkoutPlanRepository(
            IWorkoutPlanCacheService workoutPlanCacheService,
            IUserRepository userRepository,
            IExerciseRepository exerciseRepository,
            IBackgroundJobClientService backgroundJobClientService,
            IQueryProcessor queryProcessor)
        {
            _workoutPlanCacheService = workoutPlanCacheService;
            _exerciseRepository = exerciseRepository;
            _userRepository = userRepository;
            _backgroundJobClientService = backgroundJobClientService;
            _queryProcessor = queryProcessor;
        }

        public async Task AddWorkoutPlanAsync(string username, WorkoutPlanPersistanceDTO workoutPlan)
        {
            await ValidateExercisesAsync(workoutPlan);
            if (!_userRepository.DoesUserExistsInCacheAsync(username)) 
            {
                await _userRepository.AddUser(username);
            };
            AddWorkoutPlanToCache(username, workoutPlan);
            _backgroundJobClientService.Enqueue<IAddWorkoutPlanJob>(x => x.Run(username, workoutPlan));
        }

        public async Task UpdateWorkoutPlanAsync(string username, WorkoutPlanPersistanceDTO workoutPlan)
        {

            await ValidateExercisesAsync(workoutPlan);
            if (!await UserWorkoutPlanExistsAsync(username, workoutPlan.ExternalId))
            {
                throw new Exception("there is no such workout");
            };
            UpdateWorkoutInCache(username, workoutPlan);
            _backgroundJobClientService.Enqueue<IUpdateWorkoutPlanJob>(x => x.Run(username, workoutPlan));
        }

        public async Task DeleteWorkoutPlanAsync(string username, Guid externalId)
        {
            if (!await UserWorkoutPlanExistsAsync(username, externalId))
            {
                throw new Exception("there is no such workout");
            };
            DeleteWorkoutFromCache(username, externalId);
            _backgroundJobClientService.Enqueue<IDeleteWorkoutPlanJob>(x => x.Run(username, externalId));
        }

        private void AddWorkoutPlanToCache(string username, WorkoutPlanPersistanceDTO workoutPlan)
        {
            var saveWorkouts = _workoutPlanCacheService.GetUserWorkouts(username)?.Value.ToList() ??
                new List<WorkoutPlanPersistanceDTO>();
            saveWorkouts.Add(workoutPlan);
            _workoutPlanCacheService.PutWorkoutPlans(username, saveWorkouts);
        }

        private void DeleteWorkoutFromCache(string username, Guid externalId) 
        {
            var workouts = _workoutPlanCacheService.GetUserWorkouts(username).Value;
            _workoutPlanCacheService.PutWorkoutPlans(username, workouts.Where(x => x.ExternalId != externalId));
        }

        private void UpdateWorkoutInCache(string username, WorkoutPlanPersistanceDTO workoutPlan) {
            var workouts = _workoutPlanCacheService.GetUserWorkouts(username).Value;
            var saveWorkouts = workouts.Where(x => x.ExternalId != workoutPlan.ExternalId)
                .ToList();
            saveWorkouts.Add(workoutPlan);
            _workoutPlanCacheService.PutWorkoutPlans(username, saveWorkouts);
        }
        
        public Task<IEnumerable<WorkoutPlanPersistanceDTO>> GetAllUserWorkutPlansAsync(string username)
        {
            var cachedItem = _workoutPlanCacheService.GetUserWorkouts(username);
            return HandleReturnedCacheItem(cachedItem, username);
        }

        private async Task<IEnumerable<WorkoutPlanPersistanceDTO>> HandleReturnedCacheItem(CacheItem<IEnumerable<WorkoutPlanPersistanceDTO>> cachedItem, string username)
        {
            if (cachedItem == null)
            {
                return await HandleNullCacheItem(username);
            }
            else
            {
                return cachedItem.Value;
            }
        }

        private async Task<IEnumerable<WorkoutPlanPersistanceDTO>> HandleNullCacheItem(string username)
        {
            var workoutPlansFromDatabase = await _queryProcessor.Process(new GetWorkoutPlansQuery { Username = username }, default);
            _workoutPlanCacheService.PutWorkoutPlans(username, workoutPlansFromDatabase);
            return workoutPlansFromDatabase;
        }

        private async Task ValidateExercisesAsync(WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            ValidateExerciseOrder(workoutPlanPersistanceDTO);
            await ValidateExercisesNamesAsync(workoutPlanPersistanceDTO);
        }

        private async Task ValidateExercisesNamesAsync(WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            if (!workoutPlanPersistanceDTO.Exercises.All(x => exercises.Any(y => y.Id == x.ExerciseId && y.Name == x.ExerciseName)))
            {
                throw new Exception("Invalid Exercises");
            };
        }

        private void ValidateExerciseOrder(WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            if (workoutPlanPersistanceDTO.Exercises.Count()
                != workoutPlanPersistanceDTO.Exercises.Select(x => x.Order).Distinct().Count()) 
            {
                throw new Exception("Orders are not distinct");
            }
        }

        private async Task<bool> UserWorkoutPlanExistsAsync(string username, Guid externalId)
        {
            var workouts = await GetAllUserWorkutPlansAsync(username);
            return workouts.Any(x => x.ExternalId == externalId);
        }
    }
}
