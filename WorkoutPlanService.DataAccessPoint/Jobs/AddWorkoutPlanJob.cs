using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class AddWorkoutPlanJob : IAddWorkoutPlanJob
    {
        private readonly IDatabaseService _databaseService;

        public AddWorkoutPlanJob(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task Run(string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            await _databaseService.AddWorkoutPlan(username, workoutPlanPersistanceDTO);
        }
    }
}
