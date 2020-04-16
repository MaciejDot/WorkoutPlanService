using CacheManager.Core;
using System.Collections.Generic;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public interface IWorkoutPlanCacheService
    {
        public void AddWorkoutPlans(string username, IEnumerable<WorkoutPlanPersistanceDTO> workoutPlans);
        void PutWorkoutPlans(string username, IEnumerable<WorkoutPlanPersistanceDTO> workoutPlans);
        CacheItem<IEnumerable<WorkoutPlanPersistanceDTO>> GetUserWorkouts(string username);
    }
}