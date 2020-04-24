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
            if (await UserWorkoutPlanExistsAsync(workoutPlan.Name, username)) 
            {
                throw new Exception("Workout name is already taken");
            };
            if (!_userRepository.DoesUserExistsInCacheAsync(username)) 
            {
                await _userRepository.AddUser(username);
            };
            AddWorkoutPlanToCache(username, workoutPlan);
            _backgroundJobClientService.Enqueue<IAddWorkoutPlanJob>(x => x.Run(username, workoutPlan));
        }

        public async Task UpdateWorkoutPlanAsync(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlan)
        {

            await ValidateExercisesAsync(workoutPlan);
            if (!await UserWorkoutPlanExistsAsync(oldWorkoutName, username))
            {
                throw new Exception("there is no such workout");
            };
            UpdateWorkoutInCache(username, oldWorkoutName, workoutPlan);
            _backgroundJobClientService.Enqueue<IUpdateWorkoutPlanJob>(x => x.Run(username, oldWorkoutName, workoutPlan));
        }

        public async Task DeleteWorkoutPlanAsync(string username, string workoutName)
        {
            if (!await UserWorkoutPlanExistsAsync(workoutName, username))
            {
                throw new Exception("there is no such workout");
            };
            DeleteWorkoutFromCache(username, workoutName);
            _backgroundJobClientService.Enqueue<IDeleteWorkoutPlanJob>(x => x.Run(username, workoutName));
        }

        private void AddWorkoutPlanToCache(string username, WorkoutPlanPersistanceDTO workoutPlan)
        {
            var saveWorkouts = _workoutPlanCacheService.GetUserWorkouts(username)?.Value.ToList() ??
                new List<WorkoutPlanPersistanceDTO>();
            saveWorkouts.Add(workoutPlan);
            _workoutPlanCacheService.PutWorkoutPlans(username, saveWorkouts);
        }

        private void DeleteWorkoutFromCache(string username, string workoutName) 
        {
            var workouts = _workoutPlanCacheService.GetUserWorkouts(username).Value;
            _workoutPlanCacheService.PutWorkoutPlans(username, workouts.Where(x => x.Name != workoutName));
        }

        private void UpdateWorkoutInCache(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlan) {
            var workouts = _workoutPlanCacheService.GetUserWorkouts(username).Value;
            var saveWorkouts = workouts.Where(x => x.Name != oldWorkoutName)
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

        private async Task<bool> UserWorkoutPlanExistsAsync(string workoutName, string username)
        {
            var workouts = await GetAllUserWorkutPlansAsync(username);
            return workouts.Any(x => x.Name == workoutName);
        }
    }
}
