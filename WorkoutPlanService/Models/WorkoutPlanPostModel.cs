using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutPlanService.Models
{
    public sealed class WorkoutPlanPostModel
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExercisePlanModel> Exercises { get; set; }
    }
}
