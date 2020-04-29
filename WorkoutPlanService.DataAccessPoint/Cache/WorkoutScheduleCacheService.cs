using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public class WorkoutScheduleCacheService : IWorkoutScheduleCacheService
    {
        private readonly ICacheManager<IEnumerable<WorkoutScheduleDTO>> _cacheManager;

        public WorkoutScheduleCacheService(ICacheManager<IEnumerable<WorkoutScheduleDTO>> cacheManager) 
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<IEnumerable<WorkoutScheduleDTO>> Get(string username)
        {
            return _cacheManager.GetCacheItem(username);
        }

        public void Put(string username, IEnumerable<WorkoutScheduleDTO> workoutScheduleDTOs)
        {
            _cacheManager.Put(username, workoutScheduleDTOs);
        }
    }
}
