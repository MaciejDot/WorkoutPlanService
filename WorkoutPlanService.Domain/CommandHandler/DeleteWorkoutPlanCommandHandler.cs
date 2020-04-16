using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DateTimeHelper;

namespace WorkoutPlanService.Domain.CommandHandler
{
    public class DeleteWorkoutPlanCommandHandler : IRequestHandler<DeleteWorkoutPlanCommand, Unit>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IDateTimeHelper _dateTimeHelper;

        public DeleteWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository,
            IDateTimeHelper dateTimeHelper) 
        {
            _workoutPlanRepository = workoutPlanRepository;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<Unit> Handle(DeleteWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            await _workoutPlanRepository.DeleteWorkoutPlanAsync(command.Username, command.WorkoutName, _dateTimeHelper.GetCurrentDateTime());
            return new Unit();
        }
    }
}
