using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.DTO
{
    public class WorkoutPlanPersistanceDTO
    {
        [MaxLength(400)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExerciseExecutionPersistanceDTO> Exercises { get; set; }
    }
}
