﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DateTimeHelper;

namespace WorkoutPlanService.Domain.CommandHandler
{
    public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, Unit>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IDateTimeHelper _dateTimeHelper;
        public CreateWorkoutPlanCommandHandler(IWorkoutPlanRepository workoutPlanRepository, IDateTimeHelper dateTimeHelper) 
        {
            _workoutPlanRepository = workoutPlanRepository;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<Unit> Handle(CreateWorkoutPlanCommand command, CancellationToken cancellationToken) 
        {
            await _workoutPlanRepository.AddWorkoutPlanAsync(command.Username, new WorkoutPlanPersistanceDTO
            {
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
            return new Unit();
        }
    }
}
