using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public sealed class UpdateWorkoutPlanCommandHandler : ICommandHandler<UpdateWorkoutPlanCommand>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        public UpdateWorkoutPlanCommandHandler(ICommandDispatcher commandDispatcher) 
        {
            _commandDispatcher = commandDispatcher;
        }
        public Task Handle(UpdateWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            return _commandDispatcher.Dispatch(new AddWorkoutPlanCommand { Username = command.Username, WorkoutPlan = command.WorkoutPlan }, cancellationToken);
        }
    }
}
