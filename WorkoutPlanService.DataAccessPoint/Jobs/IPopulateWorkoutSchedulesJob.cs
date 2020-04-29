using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IPopulateWorkoutSchedulesJob
    {
        Task Run();
    }
}