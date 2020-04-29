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
    public class GetUserWorkoutSchedulesQueryHandler : IQueryHandler<GetUserWorkoutSchedulesQuery, IEnumerable<WorkoutScheduleDTO>>
    {
        private readonly SqlConnection _sqlConnection;
        public GetUserWorkoutSchedulesQueryHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }
        public Task<IEnumerable<WorkoutScheduleDTO>> Handle(GetUserWorkoutSchedulesQuery query, CancellationToken cancellationToken)
        {
            return _sqlConnection.QueryAsync<WorkoutScheduleDTO>("[Workout].[sp_WorkoutSchedules_Get]", query, commandType: CommandType.StoredProcedure);
        }
    }
}
