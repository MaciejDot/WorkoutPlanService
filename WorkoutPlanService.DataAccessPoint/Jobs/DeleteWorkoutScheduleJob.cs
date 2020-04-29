using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class DeleteWorkoutScheduleJob : IDeleteWorkoutScheduleJob
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public DeleteWorkoutScheduleJob(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task Run(DeleteWorkoutScheduleCommand deleteWorkoutScheduleCommand)
        {
            await _commandDispatcher.Dispatch(deleteWorkoutScheduleCommand, default);
        }
    }
}
