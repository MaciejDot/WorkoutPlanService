using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public class UserCacheService : IUserCacheService
    {
        private readonly ICacheManager<UserPersistanceDTO> _cacheManager;

        public UserCacheService(ICacheManager<UserPersistanceDTO> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public CacheItem<UserPersistanceDTO> GetUser(string username)
        {
            return _cacheManager.GetCacheItem(GetCacheKey(username));
        }

        public void AddUser(UserPersistanceDTO userPersistanceDTO)
        {
            _cacheManager.AddOrUpdate(GetCacheKey(userPersistanceDTO.Name), userPersistanceDTO, x => x);
        }

        public void PutUser(UserPersistanceDTO userPersistanceDTO)
        {
            _cacheManager.Put(GetCacheKey(userPersistanceDTO.Name), userPersistanceDTO);
        }

        private string GetCacheKey(string username)
        {
            return $"username-{username}";
        }
    }
}
