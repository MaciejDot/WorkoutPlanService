using CacheManager.Core;
using System.Collections.Generic;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public interface IWorkoutScheduleCacheService
    {
        CacheItem<IEnumerable<WorkoutScheduleDTO>> Get(string username);
        void Put(string username, IEnumerable<WorkoutScheduleDTO> workoutScheduleDTOs);
    }
}