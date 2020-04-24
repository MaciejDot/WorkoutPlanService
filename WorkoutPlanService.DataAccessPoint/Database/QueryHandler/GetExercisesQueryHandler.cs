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
    public sealed class GetExercisesQueryHandler : IQueryHandler<GetExercisesQuery, IEnumerable<ExercisePersistanceDTO>>
    {
        private readonly SqlConnection _sqlConnection;
        public GetExercisesQueryHandler(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public Task<IEnumerable<ExercisePersistanceDTO>> Handle(GetExercisesQuery query, CancellationToken cancellationToken)
        {
            return _sqlConnection.QueryAsync<ExercisePersistanceDTO>(
                "[Workout].[sp_Exercise_GetAll]",
                commandType: CommandType.StoredProcedure);
        }
    }
}
