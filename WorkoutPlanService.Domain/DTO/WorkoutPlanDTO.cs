using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.DTO
{
    public sealed class WorkoutPlanDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExerciseExecutionDTO> Exercises { get; set; }
    }
}
