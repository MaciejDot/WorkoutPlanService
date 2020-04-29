using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.DTO
{
    public struct WorkoutScheduleDTO
    {
        public Guid ExternalId { get; set; }
        public int? Recurrence { get; set; }
        public DateTime FirstDate { get; set; }
        public int? RecurringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
