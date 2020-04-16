﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutPlanService.Models
{
    public class WorkoutPlanPatchModel
    {
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExercisePlanModel> Exercises { get; set; }
    }
}
