using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SimpleCQRS.Query;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database.QueryHandler
{
    public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserPersistanceDTO>
    {
        private readonly SqlConnection _sqlConnection;
        public GetUserQueryHandler(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public Task<UserPersistanceDTO> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            return _sqlConnection.QueryFirstOrDefaultAsync<UserPersistanceDTO>("[Security].[sp_User_Get]",
                query,
                commandType: CommandType.StoredProcedure);
        }
    }
}
