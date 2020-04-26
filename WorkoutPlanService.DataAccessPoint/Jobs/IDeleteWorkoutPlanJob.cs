using System;
using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IDeleteWorkoutPlanJob
    {
        Task Run(string username, Guid ExternalId);
    }
}