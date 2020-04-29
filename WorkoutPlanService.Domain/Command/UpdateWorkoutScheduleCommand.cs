using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.Command
{
    public class UpdateWorkoutScheduleCommand : IRequest<Unit>
    {
        public Guid ExternalId { get; set; }
        public string Username { get; set; }
        public int? Recurrence { get; set; }
        public DateTime FirstDate { get; set; }
        public int? RecurringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
