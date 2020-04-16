using System;
using System.Linq.Expressions;

namespace WorkoutPlanService.DataAccessPoint.Hangfire
{
    public interface IBackgroundJobClientService
    {
        void Enqueue<T>(Expression<Action<T>> methodCall);
        void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan dateTimeOffset);
    }
}