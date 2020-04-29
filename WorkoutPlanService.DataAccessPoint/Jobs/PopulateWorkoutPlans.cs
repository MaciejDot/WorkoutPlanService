using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public sealed class PopulateWorkoutPlans : IPopulateWorkoutPlans
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IWorkoutPlanCacheService _workoutPlanCacheService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public PopulateWorkoutPlans(IQueryProcessor queryProcessor, IWorkoutPlanCacheService workoutPlanCacheService, IBackgroundJobClientService backgroundJobClientService)
        {
            _queryProcessor = queryProcessor;
            _workoutPlanCacheService = workoutPlanCacheService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            var workoutPlans = await _queryProcessor.Process(new GetAllWorkoutPlansQuery(), default);
            workoutPlans
                    .AsParallel()
                    .ForAll(workoutPlans => _workoutPlanCacheService.PutWorkoutPlans(workoutPlans.Key, workoutPlans.Value));
            _backgroundJobClientService.Schedule<IPopulateWorkoutPlans>(x => x.Run(), TimeSpan.FromMinutes(9));
        }
    }
}
