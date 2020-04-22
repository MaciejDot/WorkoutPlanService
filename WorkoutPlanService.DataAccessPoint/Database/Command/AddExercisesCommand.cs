using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database.Command
{
    public class AddExercisesCommand : ICommand
    {
        public IEnumerable<ExercisePersistanceDTO> Exercises { get; set; }
    }
}
