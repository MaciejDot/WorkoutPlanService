using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutPlanService.Models
{
    public sealed class ExercisePlanModel
    {
        public int Series { get; set; }
        public int MinReps { get; set; }
        public int MaxReps { get; set; }
        public int MinAdditionalKgs { get; set; }
        public int MaxAdditionalKgs { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public int Break { get; set; }
    }
}
