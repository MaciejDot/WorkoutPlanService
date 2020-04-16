using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    class UserRepository : IUserRepository
    {
        private readonly IUserCacheService _userCacheService;
        private readonly IDatabaseService _databaseService;

        public UserRepository(
            IUserCacheService userCacheService,
            IDatabaseService databaseService
        )
        {
            _databaseService = databaseService;
            _userCacheService = userCacheService;
        }

        public async Task AddUser(string username)
        {
            if (_userCacheService.GetUser(username) == null)
            {
                _userCacheService.PutUser(new UserPersistanceDTO { Name = username });
                if (await _databaseService.GetUser(username) == null)
                {
                    await _databaseService.AddUser(new UserPersistanceDTO { Name = username });
                }
            }
        }



        public bool DoesUserExistsInCacheAsync(string username)
        {
            return _userCacheService.GetUser(username) != null;
        }
    }
}
