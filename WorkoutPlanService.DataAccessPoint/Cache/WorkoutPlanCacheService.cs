using CacheManager.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public sealed class WorkoutPlanCacheService : IWorkoutPlanCacheService
    {
        private readonly ICacheManager<IEnumerable<WorkoutPlanPersistanceDTO>> _cacheManager;

        public WorkoutPlanCacheService(ICacheManager<IEnumerable<WorkoutPlanPersistanceDTO>> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<IEnumerable<WorkoutPlanPersistanceDTO>> GetUserWorkouts(string username)
        {
            return _cacheManager.GetCacheItem(username);
        }

        public void PutWorkoutPlans(string username, IEnumerable<WorkoutPlanPersistanceDTO> workoutPlans)
        {
            _cacheManager.Put(username, workoutPlans);
        }
    }
}
