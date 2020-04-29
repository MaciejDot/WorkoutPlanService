using CacheManager.Core;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DatetimeService;
using WorkoutPlanService.DataAccessPoint.GuidService;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;
using WorkoutPlanService.DataAccessPoint.Repositories;
using System.Reflection;
using SimpleCQRS.DependencyInjectionExtensions;

namespace WorkoutPlanService.DataAccessPoint.Configuration
{
    public static class WorkoutPlanServiceDataAccessPointDependencyInjectionExtension
    {
        public static IServiceCollection AddWorkoutPlanServiceDataAccessPointOptions(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<ExerciseServiceOptions>(configuration);
            return services;
        }


        public static IServiceCollection AddWorkoutPlanServiceDataAccessPoint(this IServiceCollection services, string sqlConnectionString)
        {
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<IWorkoutScheduleCacheService, WorkoutScheduleCacheService>();
            services.AddSingleton<IWorkoutSchedulesRepository, WorkoutSchedulesRepository>();
            services.AddTransient<IWorkoutPlanRepository, WorkoutPlanRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExerciseRepository, ExerciseRepository>();
            services.AddTransient(_ => new SqlConnection(sqlConnectionString));
            services.AddTransient<IWorkoutPlanCacheService, WorkoutPlanCacheService>();
            services.AddTransient<IUserCacheService, UserCacheService>();
            services.AddTransient<IExerciseCacheService, ExerciseCacheService>();
            services.AddSingleton<IGuidProvider, GuidProvider>();
            services.AddSingleton<IBackgroundJobClientService, BackgroundJobClientService>();
            services.AddTransient<IAddWorkoutPlanJob, AddWorkoutPlanJob>();
            services.AddTransient<IUpdateWorkoutPlanJob, UpdateWorkoutPlanJob>();
            services.AddTransient<IDeleteWorkoutPlanJob, DeleteWorkoutPlanJob>();
            services.AddTransient<IUpdateExercisesJob, UpdateExercisesJob>();
            services.AddTransient<IPopulateUserCacheJob, PopulateUserCacheJob>();
            services.AddTransient<IPopulateWorkoutPlans, PopulateWorkoutPlans>();
            services.AddTransient<IAddWorkoutScheduleJob, AddWorkoutScheduleJob>();
            services.AddTransient<IPopulateWorkoutSchedulesJob, PopulateWorkoutSchedulesJob>();
            services.AddTransient<IDeleteWorkoutScheduleJob, DeleteWorkoutScheduleJob>();
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSimpleCQRS(Assembly.GetExecutingAssembly());
            services.AddHangfire(configuration =>
            {
                configuration
                    .UseMemoryStorage();
            });
            services.AddCacheManagerConfiguration(configure =>
                configure
                .WithMicrosoftMemoryCacheHandle()
                .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(10)));
            services.AddCacheManager();
            return services;

        }
    }
}
