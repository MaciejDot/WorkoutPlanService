using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public sealed class DeleteExercisesCommand : ICommand
    {
        public IEnumerable<int> Ids { get; set; }
    }
}
