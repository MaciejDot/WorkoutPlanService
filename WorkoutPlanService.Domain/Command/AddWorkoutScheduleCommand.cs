using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.Domain.DTO;

namespace WorkoutPlanService.Domain.Command
{
    public class AddWorkoutScheduleCommand : IRequest<WorkoutScheduleIdentityDTO>
    {
        public string Username { get; set; }
        public int? Recurrence { get; set; }
        public DateTime FirstDate { get; set; }
        public int? RecurringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
