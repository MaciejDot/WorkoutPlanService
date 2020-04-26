using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.Domain.Models;

namespace WorkoutPlanService.Domain.Command
{
    public sealed class UpdateWorkoutPlanCommand : IRequest<Unit>
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExercisePlanCommandModel> Exercises { get; set; }
    }
}
