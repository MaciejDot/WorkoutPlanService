using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IDeleteWorkoutScheduleJob
    {
        Task Run(DeleteWorkoutScheduleCommand deleteWorkoutScheduleCommand);
    }
}