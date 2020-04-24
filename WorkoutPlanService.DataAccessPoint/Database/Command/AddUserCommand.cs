using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public sealed class AddUserCommand : ICommand
    {
        public string Name { get; set; }
    }
}
