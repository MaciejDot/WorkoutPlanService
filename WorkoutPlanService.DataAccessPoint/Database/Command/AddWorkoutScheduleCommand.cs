using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public class AddWorkoutScheduleCommand : ICommand
    {
        public DateTime Created { get; set; }
        public Guid ExternalId { get; set; }
        public int? Recurrence { get; set; }
        public DateTime FirstDate { get; set; }
        public int? RecurringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
