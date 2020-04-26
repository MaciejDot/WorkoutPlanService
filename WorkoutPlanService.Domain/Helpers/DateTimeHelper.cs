using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.Helpers
{
    public sealed class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
