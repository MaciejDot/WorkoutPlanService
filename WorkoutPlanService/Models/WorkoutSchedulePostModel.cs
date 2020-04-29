using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkoutPlanService.Models
{
    public struct WorkoutSchedulePostModel
    {
        public int? Recurrence { get; set; }
        public DateTime FirstDate { get; set; }
        public int? RecurringTimes { get; set; }
        public Guid WorkoutPlanExternalId { get; set; }
    }
}
