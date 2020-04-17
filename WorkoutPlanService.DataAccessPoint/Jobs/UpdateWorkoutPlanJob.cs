using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class UpdateWorkoutPlanJob : IUpdateWorkoutPlanJob
    {
        private readonly IDatabaseService _databaseService;
        public UpdateWorkoutPlanJob(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task Run(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlan)
        {
            await _databaseService.UpdateWorkoutPlan(username, oldWorkoutName, workoutPlan);
        }
    }
}
