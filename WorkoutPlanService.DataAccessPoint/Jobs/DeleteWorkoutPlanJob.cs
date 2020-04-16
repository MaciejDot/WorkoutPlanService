using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class DeleteWorkoutPlanJob : IDeleteWorkoutPlanJob
    {
        public IDatabaseService _databaseService;

        public DeleteWorkoutPlanJob(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task Run(string username, string workoutName, DateTime deactivationDate)
        {
            await _databaseService.DeleteWorkoutPlan(username, workoutName, deactivationDate);
        }
    }
}
