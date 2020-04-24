using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WorkoutPlanService.Domain.DateTimeHelper;

namespace WorkoutPlanService.Domain.Configuration
{
    public static class WorkoutPlanServiceDomainDependencyInjectionExtension
    {
        public static IServiceCollection AddWorkoutPlanServiceDomain(this IServiceCollection services) {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IDateTimeHelper, DateTimeHelper.DateTimeHelper>();
            return services;
        }
    }
}
