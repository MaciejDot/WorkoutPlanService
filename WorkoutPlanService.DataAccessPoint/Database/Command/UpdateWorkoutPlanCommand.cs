using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public sealed class UpdateWorkoutPlanCommand : ICommand
    {
        public WorkoutPlanPersistanceDTO WorkoutPlan { get; set; }
        public string Username { get; set; }
    }
}
