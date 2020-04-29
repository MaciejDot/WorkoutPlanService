using CacheManager.Core;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Cache
{
    public interface IUserCacheService
    {
        CacheItem<UserPersistanceDTO> GetUser(string username);
        void PutUser(UserPersistanceDTO userPersistanceDTO);
    }
}