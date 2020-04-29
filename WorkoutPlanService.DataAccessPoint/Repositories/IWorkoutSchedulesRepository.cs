using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public interface IWorkoutSchedulesRepository
    {
        Task<Guid> AddWorkoutScheduleAsync(string username, WorkoutScheduleDTO workoutScheduleDTO);
        Task<IEnumerable<WorkoutScheduleDTO>> GetWorkoutSchedules(string username);
        Task UpdateWorkoutScheduleAsync(string username, WorkoutScheduleDTO workoutScheduleDTO);
        Task DeleteWorkoutScheduleAsync(string username, Guid externalId);
    }
}