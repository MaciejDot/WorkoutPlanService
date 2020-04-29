using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Helpers;

namespace WorkoutPlanService.Domain.CommandHandler
{
    class AddWorkoutScheduleCommandHandler : IRequestHandler<AddWorkoutScheduleCommand, WorkoutScheduleIdentityDTO>
    {
        private readonly IWorkoutSchedulesRepository _workoutSchedulesRepository;
        private readonly IGuidProvider _guidProvider;
        public AddWorkoutScheduleCommandHandler(
            IWorkoutSchedulesRepository workoutSchedulesRepository,
            IGuidProvider guidProvider)
        {
            _workoutSchedulesRepository = workoutSchedulesRepository;
            _guidProvider = guidProvider;
        }
        public async Task<WorkoutScheduleIdentityDTO> Handle(AddWorkoutScheduleCommand request, CancellationToken cancellationToken)
        {
            return new WorkoutScheduleIdentityDTO
            {
                ExternalId = await _workoutSchedulesRepository.AddWorkoutScheduleAsync(request.Username, new DataAccessPoint.DTO.WorkoutScheduleDTO
                {
                    WorkoutPlanExternalId = request.WorkoutPlanExternalId,
                    FirstDate = request.FirstDate,
                    Recurrence = request.Recurrence,
                    RecurringTimes = request.RecurringTimes,
                    ExternalId = _guidProvider.GetGuid()
                })
            };
        }
    }
}
