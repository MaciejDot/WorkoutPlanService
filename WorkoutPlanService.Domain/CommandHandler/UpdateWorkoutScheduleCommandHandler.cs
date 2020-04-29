using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;

namespace WorkoutPlanService.Domain.CommandHandler
{
    public class UpdateWorkoutScheduleCommandHandler : IRequestHandler<UpdateWorkoutScheduleCommand, Unit>
    {
        private readonly IWorkoutSchedulesRepository _workoutSchedulesRepository;
        public UpdateWorkoutScheduleCommandHandler(IWorkoutSchedulesRepository workoutSchedulesRepository)
        {
            _workoutSchedulesRepository = workoutSchedulesRepository;
        }
        public async Task<Unit> Handle(UpdateWorkoutScheduleCommand request, CancellationToken cancellationToken)
        {
            await _workoutSchedulesRepository.UpdateWorkoutScheduleAsync(request.Username, new WorkoutScheduleDTO 
            { 
                ExternalId = request.ExternalId,
                FirstDate = request.FirstDate,
                Recurrence = request.Recurrence,
                RecurringTimes = request.RecurringTimes,
                WorkoutPlanExternalId = request.WorkoutPlanExternalId
            });
            return new Unit();
        }
    }
}
