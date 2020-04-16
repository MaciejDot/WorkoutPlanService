using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class PopulateUserCacheJob : IPopulateUserCacheJob
    {
        private readonly IDatabaseService _databaseService;
        private readonly IUserCacheService _userCacheService;
        public PopulateUserCacheJob(IDatabaseService databaseService, IUserCacheService userCacheService)
        {
            _databaseService = databaseService;
            _userCacheService = userCacheService;
        }

        public async Task Run()
        {
            var users = await _databaseService.GetAllUsers();
            users
                .AsParallel()
                .ForAll(user => _userCacheService.AddUser(user));
        }
    }
}
