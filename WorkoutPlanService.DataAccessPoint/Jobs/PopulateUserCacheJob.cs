using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class PopulateUserCacheJob : IPopulateUserCacheJob
    {
        private readonly IDatabaseService _databaseService;
        private readonly IUserCacheService _userCacheService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;
        public PopulateUserCacheJob(IDatabaseService databaseService, IUserCacheService userCacheService, IBackgroundJobClientService backgroundJobClientService)
        {
            _databaseService = databaseService;
            _userCacheService = userCacheService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            var users = await _databaseService.GetAllUsers();
            users
                .AsParallel()
                .ForAll(user => _userCacheService.AddUser(user));
            _backgroundJobClientService.Schedule<IPopulateUserCacheJob>(x => x.Run(), TimeSpan.FromMinutes(9));
        }
    }
}
