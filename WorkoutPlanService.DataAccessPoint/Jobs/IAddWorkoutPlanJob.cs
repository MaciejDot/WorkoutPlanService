using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IAddWorkoutPlanJob
    {
        Task Run(string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO);
    }
}