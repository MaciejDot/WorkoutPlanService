using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public struct DeleteWorkoutScheduleCommand : ICommand
    {
        public DateTime Created { get; set; }
        public Guid ExternalId { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
