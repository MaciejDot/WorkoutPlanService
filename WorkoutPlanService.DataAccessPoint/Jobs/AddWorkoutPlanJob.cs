using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public sealed class AddWorkoutPlanJob : IAddWorkoutPlanJob
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public AddWorkoutPlanJob(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Run(string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            await _commandDispatcher.Dispatch(new AddWorkoutPlanCommand { Username = username, WorkoutPlan = workoutPlanPersistanceDTO }, default);
        }
    }
}
