using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.DTO
{
    public sealed class ExerciseExecutionPersistanceDTO
    {
        public int Series { get; set; }
        public int MinReps { get; set; }
        public int MaxReps { get; set; }
        public int MinAdditionalKgs { get; set; }
        public int MaxAdditionalKgs { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public int Order { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public int Break { get; set; }
    }
}
