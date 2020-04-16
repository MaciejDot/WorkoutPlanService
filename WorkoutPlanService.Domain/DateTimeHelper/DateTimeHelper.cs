using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.DateTimeHelper
{
    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
