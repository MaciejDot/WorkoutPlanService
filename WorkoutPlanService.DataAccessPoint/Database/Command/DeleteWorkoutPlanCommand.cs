using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public class DeleteWorkoutPlanCommand : ICommand
    {
        public string WorkoutName { get; set; }
        public string Username { get; set; }
    }
}
