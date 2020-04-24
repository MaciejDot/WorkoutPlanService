using SimpleCQRS.Command;
using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;
using WorkoutPlanService.DataAccessPoint.Jobs;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IUserCacheService _userCacheService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryProcessor _queryProcessor;
        public UserRepository(
            IUserCacheService userCacheService,
            ICommandDispatcher commandDispatcher,
            IQueryProcessor queryProcessor
        )
        {
            _userCacheService = userCacheService;
            _commandDispatcher = commandDispatcher;
            _queryProcessor = queryProcessor;
        }

        public async Task AddUser(string username)
        {
            if (_userCacheService.GetUser(username) == null)
            {
                _userCacheService.PutUser(new UserPersistanceDTO { Name = username });
                if (await _queryProcessor.Process(new GetUserQuery { Name = username }, default) == null)
                {
                     await _commandDispatcher.Dispatch(new AddUserCommand{ Name = username }, default);
                }
            }
        }



        public bool DoesUserExistsInCacheAsync(string username)
        {
            return _userCacheService.GetUser(username) != null;
        }
    }
}
