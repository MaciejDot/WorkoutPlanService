using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class PopulateWorkoutSchedulesJob : IPopulateWorkoutSchedulesJob
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IWorkoutScheduleCacheService _workoutScheduleCacheService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public PopulateWorkoutSchedulesJob(IQueryProcessor queryProcessor,
            IWorkoutScheduleCacheService workoutScheduleCacheService,
            IBackgroundJobClientService backgroundJobClientService)
        {
            _queryProcessor = queryProcessor;
            _workoutScheduleCacheService = workoutScheduleCacheService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            var schedules = await _queryProcessor.Process(new GetAllWorkoutSchedulesQuery(), default);
            schedules
                .AsParallel()
                .ForAll(entry =>
                    _workoutScheduleCacheService.Put(entry.Key, entry.Value)
                );
            _backgroundJobClientService.Schedule<IPopulateWorkoutSchedulesJob>(x => x.Run(), TimeSpan.FromMinutes(10));
        }
    }
}
