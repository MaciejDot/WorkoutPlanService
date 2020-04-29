using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using SimpleCQRS.Query;
using WorkoutPlanService.DataAccessPoint.Database.Query;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public sealed class PopulateUserCacheJob : IPopulateUserCacheJob
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserCacheService _userCacheService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;
        public PopulateUserCacheJob(IQueryProcessor queryProcessor, 
            IUserCacheService userCacheService,
            IBackgroundJobClientService backgroundJobClientService)
        {
            _queryProcessor = queryProcessor;
            _userCacheService = userCacheService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            var users = await _queryProcessor.Process( new GetUsersQuery(), default);
            users
                .AsParallel()
                .ForAll(user => _userCacheService.PutUser(user));
            _backgroundJobClientService.Schedule<IPopulateUserCacheJob>(x => x.Run(), TimeSpan.FromMinutes(9));
        }
    }
}
