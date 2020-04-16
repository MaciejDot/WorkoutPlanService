using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public class ExerciseCacheService : IExerciseCacheService
    {
        private readonly ICacheManager<IEnumerable<ExercisePersistanceDTO>> _cacheManager;

        public ExerciseCacheService(ICacheManager<IEnumerable<ExercisePersistanceDTO>> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<IEnumerable<ExercisePersistanceDTO>> GetAllExercises()
        {
            return _cacheManager.GetCacheItem(GetCacheKey());
        }

        public void PutExercises(IEnumerable<ExercisePersistanceDTO> exercisePersistanceDTOs)
        {
            _cacheManager.Put(GetCacheKey(), exercisePersistanceDTOs);
        }

        private string GetCacheKey()
        {
            return $"exercises";
        }
    }
}
