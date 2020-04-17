using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database
{
    public interface IDatabaseService
    {
        Task DeleteExercise(ExercisePersistanceDTO exercisePersistanceDTO);
        Task AddExercise(ExercisePersistanceDTO exercisePersistanceDTO);
        Task<IEnumerable<WorkoutPlanPersistanceDTO>> GetWoroutPlansAsync(string username);
        Task AddWorkoutPlan(string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO);
        Task UpdateWorkoutPlan(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO);
        Task DeleteWorkoutPlan(string username, string workoutName, DateTime deactivationDate);
        public Task<UserPersistanceDTO> GetUser(string username);
        public Task<IEnumerable<UserPersistanceDTO>> GetUsers();
        public Task AddUser(UserPersistanceDTO userPersistanceDTO);
        public Task<IEnumerable<ExercisePersistanceDTO>> GetExercises();
        public Task<IEnumerable<UserPersistanceDTO>> GetAllUsers();
        public Task<IDictionary<string, IEnumerable<WorkoutPlanPersistanceDTO>>> GetAllWorkoutPlans();
    }
}