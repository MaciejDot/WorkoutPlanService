using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.Domain.DTO;

namespace WorkoutPlanService.Domain.Query
{
    public sealed class GetSpecificWorkoutQuery : IRequest<WorkoutPlanDTO>
    {
        public string IssuerName { get; set; }
        public string OwnerName { get; set; }
        public Guid ExternalId { get; set; }
    }
}
