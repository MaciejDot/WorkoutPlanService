using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Query;

namespace WorkoutPlanService.Domain.QueryHandler
{
    public sealed class GetSpecificWorkoutQueryHandler : IRequestHandler<GetSpecificWorkoutQuery, WorkoutPlanDTO>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public GetSpecificWorkoutQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
        }

        public async Task<WorkoutPlanDTO> Handle(GetSpecificWorkoutQuery query, CancellationToken cancellationToken)
        {
            var workoutPlans = await _workoutPlanRepository.GetAllUserWorkutPlansAsync(query.OwnerName);
            var workoutPlan = workoutPlans
                .Select(x => new WorkoutPlanDTO
                {
                    ExternalId = x.ExternalId,
                    Name = x.Name,
                    Created = x.Created,
                    Description = x.Description,
                    IsPublic = x.IsPublic,
                    Exercises = x.Exercises.Select(y=> new ExerciseExecutionDTO {
                        ExerciseName = y.ExerciseName,
                        ExerciseId = y.ExerciseId,
                        Break = y.Break,
                        Description = y.Description,
                        MaxAdditionalKgs = y.MaxAdditionalKgs,
                        MinAdditionalKgs = y.MinAdditionalKgs,
                        MinReps = y.MinReps,
                        MaxReps = y.MaxReps,
                        Order = y.Order,
                        Series = y.Series
                    })
                })
                .First(x => x.ExternalId == query.ExternalId);
            if (workoutPlan.IsPublic || query.OwnerName == query.IssuerName)
            {
                return workoutPlan;
            }
            throw new Exception("User is not authorized to see this workout");
        }
    }
}
