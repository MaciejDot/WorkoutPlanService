using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Helpers;

namespace WorkoutPlanService.Domain.CommandHandler
{
    public sealed class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, WorkoutPlanIdentityDTO>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGuidProvider _guidProvider;
        public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, 
            IDateTimeHelper dateTimeHelper,
            IGuidProvider guidProvider) 
        {
            _workoutPlanRepository = workoutPlanRepository;
            _dateTimeHelper = dateTimeHelper;
            _guidProvider = guidProvider;

        }

        public async Task<WorkoutPlanIdentityDTO> Handle(CreateWorkoutPlanCommand command, CancellationToken cancellationToken) 
        {
            var identity = new WorkoutPlanIdentityDTO { ExternalId = _guidProvider.GetGuid() };
            await _workoutPlanRepository.AddWorkoutPlanAsync(command.Username, new WorkoutPlanPersistanceDTO
            {
                ExternalId = identity.ExternalId,
                Name = command.Name,
                Created = _dateTimeHelper.GetCurrentDateTime(),
                Description = command.Description,
                IsPublic = command.IsPublic,
                Exercises = command.Exercises.Select(x => new ExerciseExecutionPersistanceDTO
                {
                    ExerciseId = x.ExerciseId,
                    ExerciseName = x.ExerciseName,
                    Break = x.Break,
                    Description = x.Description,
                    MaxAdditionalKgs = x.MaxAdditionalKgs,
                    MinAdditionalKgs = x.MinAdditionalKgs,
                    MinReps = x.MinReps,
                    MaxReps = x.MaxReps,
                    Order = x.Order,
                    Series = x.Series

                })
            });
            return identity;
        }
    }
}
