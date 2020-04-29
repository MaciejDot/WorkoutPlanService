using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;

namespace WorkoutPlanService.Domain.CommandHandler
{
    public class DeleteWorkoutScheduleCommandHandler : IRequestHandler<DeleteWorkoutScheduleCommand, Unit>
    {
        private readonly IWorkoutSchedulesRepository _workoutSchedulesRepository;
        public DeleteWorkoutScheduleCommandHandler(IWorkoutSchedulesRepository workoutSchedulesRepository)
        {
            _workoutSchedulesRepository = workoutSchedulesRepository;
        }
        public async Task<Unit> Handle(DeleteWorkoutScheduleCommand request, CancellationToken cancellationToken)
        {
            await _workoutSchedulesRepository.DeleteWorkoutScheduleAsync(request.Username, request.ExternalId);
            return new Unit();
        }
    }
}
