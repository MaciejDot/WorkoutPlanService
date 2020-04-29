using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.Domain.DTO;

namespace WorkoutPlanService.Domain.Query
{
    public class GetWorkoutSchedulesQuery : IRequest<IEnumerable<WorkoutScheduleDTO>>
    {
        public string Username { get; set; }
    }
}
