using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public sealed class DeleteWorkoutPlanCommand : ICommand
    {
        public Guid ExternalId { get; set; }
        public string Username { get; set; }
    }
}
