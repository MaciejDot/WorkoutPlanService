using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class PopulateWorkoutPlans : IPopulateWorkoutPlans
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkoutPlanCacheService _workoutPlanCacheService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public PopulateWorkoutPlans(IDatabaseService databaseService, IWorkoutPlanCacheService workoutPlanCacheService, IBackgroundJobClientService backgroundJobClientService)
        {
            _databaseService = databaseService;
            _workoutPlanCacheService = workoutPlanCacheService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            var workoutPlans = await _databaseService.GetAllWorkoutPlans();
            workoutPlans
                    .AsParallel()
                    .ForAll(workoutPlans => _workoutPlanCacheService.AddWorkoutPlans(workoutPlans.Key, workoutPlans.Value));
            _backgroundJobClientService.Schedule<IPopulateWorkoutPlans>(x => x.Run(), TimeSpan.FromMinutes(9));
        }
    }
}
