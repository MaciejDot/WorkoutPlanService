using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class PopulateWorkoutPlans : IPopulateWorkoutPlans
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkoutPlanCacheService _workoutPlanCacheService;

        public PopulateWorkoutPlans(IDatabaseService databaseService, IWorkoutPlanCacheService workoutPlanCacheService)
        {
            _databaseService = databaseService;
            _workoutPlanCacheService = workoutPlanCacheService;
        }

        public async Task Run()
        {
            var workoutPlans = await _databaseService.GetAllWorkoutPlans();
            workoutPlans
                    .AsParallel()
                    .ForAll(workoutPlans => _workoutPlanCacheService.AddWorkoutPlans(workoutPlans.Key, workoutPlans.Value));
        }
    }
}
