using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public interface IUserRepository
    {
        public bool DoesUserExistsInCacheAsync(string username);
        public Task AddUser(string username);
    }
}
