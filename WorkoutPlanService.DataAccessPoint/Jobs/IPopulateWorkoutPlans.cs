using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IPopulateWorkoutPlans
    {
        Task Run();
    }
}