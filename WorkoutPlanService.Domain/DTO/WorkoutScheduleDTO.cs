using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.DTO
{
    public struct WorkoutScheduleDTO
    {
        public Guid ExternalId { get; set; }
        public int? Recurrance { get; set; }
        public DateTime FirstDate { get; set; }
        public int? ReccuringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
        public string WorkoutPlanName { get; set; }
    }
}
