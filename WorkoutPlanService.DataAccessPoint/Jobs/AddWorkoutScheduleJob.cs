using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class AddWorkoutScheduleJob : IAddWorkoutScheduleJob
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public AddWorkoutScheduleJob(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task Run(AddWorkoutScheduleCommand addWorkoutScheduleCommand)
        {
            await _commandDispatcher.Dispatch(addWorkoutScheduleCommand, default);
        }
    }
}
