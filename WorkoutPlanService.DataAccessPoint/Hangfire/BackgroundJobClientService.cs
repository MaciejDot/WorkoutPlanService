using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.Hangfire
{
    public sealed class BackgroundJobClientService : IBackgroundJobClientService
    {
        public void Enqueue<T>(Expression<Action<T>> methodCall)
        {
            BackgroundJob.Enqueue<T>(methodCall);
        }

        public void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan dateTimeOffset) 
        {
            BackgroundJob.Schedule<T>(methodCall, dateTimeOffset);
        }
    }
}
