using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Query;
using System.Linq;

namespace WorkoutPlanService.Domain.QueryHandler
{
    public sealed class GetAllUserWorkoutPlansQueryHandler : IRequestHandler<GetAllUserWorkoutPlansQuery, IEnumerable<WorkoutPlanThumbnailDTO>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public GetAllUserWorkoutPlansQueryHandler(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
        }

        public async Task<IEnumerable<WorkoutPlanThumbnailDTO>> Handle(GetAllUserWorkoutPlansQuery query, CancellationToken cancellationToken)
        {
            var workoutPlans = await _workoutPlanRepository.GetAllUserWorkutPlansAsync(query.Username);
            return workoutPlans.Select(x => new WorkoutPlanThumbnailDTO
            {
                ExternalId = x.ExternalId,
                Name = x.Name,
                Created = x.Created,
                Description = x.Description
            });
        }
    }
}
