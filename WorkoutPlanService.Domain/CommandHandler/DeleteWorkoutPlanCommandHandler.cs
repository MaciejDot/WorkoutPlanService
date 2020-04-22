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
        public DeleteWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository) 
        {
            _workoutPlanRepository = workoutPlanRepository;
        }

        public async Task<Unit> Handle(DeleteWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            await _workoutPlanRepository.DeleteWorkoutPlanAsync(command.Username, command.WorkoutName);
            return new Unit();
        }
    }
}
