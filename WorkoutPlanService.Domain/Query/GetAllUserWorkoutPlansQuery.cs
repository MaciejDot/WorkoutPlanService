using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.Domain.DTO;

namespace WorkoutPlanService.Domain.Query
{
    public class GetAllUserWorkoutPlansQuery : IRequest<IEnumerable<WorkoutPlanThumbnailDTO>>
    {
        public string Username { get; set; }
    }
}
