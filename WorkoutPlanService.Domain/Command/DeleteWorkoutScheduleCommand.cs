using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.Command
{
    public class DeleteWorkoutScheduleCommand : IRequest<Unit>
    {
        public string Username { get; set; }
        public Guid ExternalId { get; set; }
    }
}
