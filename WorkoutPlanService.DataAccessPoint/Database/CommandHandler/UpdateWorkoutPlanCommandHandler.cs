using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public class UpdateWorkoutPlanCommandHandler : ICommandHandler<UpdateWorkoutPlanCommand>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public UpdateWorkoutPlanCommandHandler(ICommandDispatcher commandDispatcher) 
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task Handle(UpdateWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            if (command.OldWorkoutName != command.WorkoutPlan.Name)
            {
                await _commandDispatcher.Dispatch(new DeleteWorkoutPlanCommand { Username = command.Username, WorkoutName = command.OldWorkoutName }, cancellationToken);
            }
            await _commandDispatcher.Dispatch(new AddWorkoutPlanCommand { Username = command.Username, WorkoutPlan = command.WorkoutPlan }, cancellationToken);
        }
    }
}
