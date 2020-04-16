﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.Domain.Models;

namespace WorkoutPlanService.Domain.Command
{
    public class CreateWorkoutPlanCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<ExercisePlanCommandModel> Exercises { get; set; }
    }
}
