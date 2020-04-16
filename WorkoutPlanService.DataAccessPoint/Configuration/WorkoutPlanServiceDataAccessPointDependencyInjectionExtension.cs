using CacheManager.Core;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.GuidService;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;
using WorkoutPlanService.DataAccessPoint.Repositories;

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
            services.AddScoped<IRestClient, RestClient>();
            services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped(_ => new SqlConnection(sqlConnectionString));
            services.AddScoped<IWorkoutPlanCacheService, WorkoutPlanCacheService>();
            services.AddScoped<IUserCacheService, UserCacheService>();
            services.AddScoped<IExerciseCacheService, ExerciseCacheService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IGuidProvider, GuidProvider>();
            services.AddScoped<IBackgroundJobClientService, BackgroundJobClientService>();
            services.AddScoped<IAddWorkoutPlanJob, AddWorkoutPlanJob>();
            services.AddScoped<IUpdateWorkoutPlanJob, UpdateWorkoutPlanJob>();
            services.AddScoped<IDeleteWorkoutPlanJob, DeleteWorkoutPlanJob>();
            services.AddScoped<IUpdateExercisesJob, UpdateExercisesJob>();
            services.AddScoped<IPopulateUserCacheJob, PopulateUserCacheJob>();
            services.AddScoped<IPopulateWorkoutPlans, PopulateWorkoutPlans>();
            services.AddHangfire(configuration =>
            {
                configuration
                    .UseMemoryStorage();
            });
            services.AddCacheManagerConfiguration(configure =>
                configure
                .WithMicrosoftMemoryCacheHandle()
                .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromDays(7)));
            services.AddCacheManager();
            return services;

        }
    }
}
