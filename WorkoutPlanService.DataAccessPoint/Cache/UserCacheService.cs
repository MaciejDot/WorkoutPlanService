using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public sealed class UserCacheService : IUserCacheService
    {
        private readonly ICacheManager<UserPersistanceDTO> _cacheManager;

        public UserCacheService(ICacheManager<UserPersistanceDTO> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<UserPersistanceDTO> GetUser(string username)
        {
            return _cacheManager.GetCacheItem(username);
        }

        public void PutUser(UserPersistanceDTO userPersistanceDTO)
        {
            _cacheManager.Put(userPersistanceDTO.Name, userPersistanceDTO);
        }

    }
}
