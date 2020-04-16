using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IUpdateWorkoutPlanJob
    {
        Task Run(string username, WorkoutPlanPersistanceDTO workoutPlan);
    }
}