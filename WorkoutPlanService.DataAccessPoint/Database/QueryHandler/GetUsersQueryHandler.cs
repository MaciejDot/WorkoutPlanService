using Dapper;
using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database.QueryHandler
{
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserPersistanceDTO>>
    {
        private readonly SqlConnection _sqlConnection;

        public GetUsersQueryHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }

        public Task<IEnumerable<UserPersistanceDTO>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            return _sqlConnection.QueryAsync<UserPersistanceDTO>(
               "[Security].[sp_User_GetAll]",
              commandType: CommandType.StoredProcedure);
        }
    }
}
