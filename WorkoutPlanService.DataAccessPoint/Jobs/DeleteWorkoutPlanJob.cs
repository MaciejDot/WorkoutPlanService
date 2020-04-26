using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public sealed class DeleteWorkoutPlanJob : IDeleteWorkoutPlanJob
    {
        public ICommandDispatcher _commandDispatcher;

        public DeleteWorkoutPlanJob(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Run(string username, Guid externalId)
        {
            await _commandDispatcher.Dispatch(new DeleteWorkoutPlanCommand { Username = username, ExternalId = externalId },default);
        }
    }
}
