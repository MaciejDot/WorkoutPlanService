using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.Command
{
    public class DeleteWorkoutPlanCommand : IRequest<Unit>
    {
        public string Username { get; set; }
        public string WorkoutName { get; set; }
    }
}
