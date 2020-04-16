using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IPopulateUserCacheJob
    {
        Task Run();
    }
}