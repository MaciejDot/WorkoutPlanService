using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.Jobs;

namespace WorkoutPlanService.DataAccessPoint.Configuration
{
    public static class WorkoutPlanServiceDataAccessPointApplicationBuilderExtension
    {
        public static IApplicationBuilder UseWorkoutPlanServiceDataAccessPoint(this IApplicationBuilder app, IBackgroundJobClient backgroundJobClient) 
        {
            app.UseHangfireServer();
            backgroundJobClient.Enqueue<IUpdateExercisesJob>(x => x.Run());
            backgroundJobClient.Enqueue<IPopulateUserCacheJob>(x => x.Run());
            backgroundJobClient.Enqueue<IPopulateWorkoutPlans>(x => x.Run());
            backgroundJobClient.Enqueue<IPopulateWorkoutSchedulesJob>(x => x.Run());
            return app;
        } 
        
    }
}
