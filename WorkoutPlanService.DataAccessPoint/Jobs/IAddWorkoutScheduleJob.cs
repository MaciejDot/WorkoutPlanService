using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IAddWorkoutScheduleJob
    {
        Task Run(AddWorkoutScheduleCommand addWorkoutScheduleCommand);
    }
}