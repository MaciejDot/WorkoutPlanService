using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public interface IWorkoutPlanRepository
    {
        Task<IEnumerable<WorkoutPlanPersistanceDTO>> GetAllUserWorkutPlansAsync(string username);
        Task AddWorkoutPlanAsync(string username, WorkoutPlanPersistanceDTO workoutPlan);
        Task UpdateWorkoutPlanAsync(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlan);
        Task DeleteWorkoutPlanAsync(string username, string workoutName);
    }
}