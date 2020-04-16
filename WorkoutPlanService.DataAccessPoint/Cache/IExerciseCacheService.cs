using CacheManager.Core;
using System.Collections.Generic;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public interface IExerciseCacheService
    {
        CacheItem<IEnumerable<ExercisePersistanceDTO>> GetAllExercises();
        void PutExercises(IEnumerable<ExercisePersistanceDTO> exercisePersistanceDTOs);
    }
}