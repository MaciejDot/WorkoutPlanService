using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.DatetimeService
{
    public sealed class DateTimeService : IDateTimeService
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }
    }
}
