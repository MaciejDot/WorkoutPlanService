using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public sealed class ExerciseCacheService : IExerciseCacheService
    {
        private readonly ICacheManager<IEnumerable<ExercisePersistanceDTO>> _cacheManager;
        private static readonly string _cacheKey = nameof(ExercisePersistanceDTO);
        public ExerciseCacheService(ICacheManager<IEnumerable<ExercisePersistanceDTO>> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<IEnumerable<ExercisePersistanceDTO>> GetAllExercises()
        {
            return _cacheManager.GetCacheItem(_cacheKey);
        }

        public void PutExercises(IEnumerable<ExercisePersistanceDTO> exercisePersistanceDTOs)
        {
            _cacheManager.Put(_cacheKey, exercisePersistanceDTOs);
        }
    }
}
