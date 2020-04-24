using Dapper;
using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Database.QueryHandler
{
    public sealed class GetWorkoutPlansQueryHandler : IQueryHandler<GetWorkoutPlansQuery, IEnumerable<WorkoutPlanPersistanceDTO>>
    {
        private readonly SqlConnection _sqlConnection;

        public GetWorkoutPlansQueryHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<IEnumerable<WorkoutPlanPersistanceDTO>> Handle(GetWorkoutPlansQuery query, CancellationToken cancellationToken)
        {
            var rawPlans = await GetRawWorkoutPlansFromDatabase(query.Username);
            return FormatFetchedWorkouts(rawPlans);
        }

        private IEnumerable<WorkoutPlanPersistanceDTO> FormatFetchedWorkouts(IEnumerable<(WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)> rawWorkoutPlans)
        {
            return rawWorkoutPlans
                    .GroupBy(x => new
                    {
                        x.Item1.Name,
                        x.Item1.Created,
                        x.Item1.Description,
                        x.Item1.IsPublic
                    })
                    .Select(x => new WorkoutPlanPersistanceDTO
                    {
                        Name = x.Key.Name,
                        Created = x.Key.Created,
                        Description = x.Key.Description,
                        Exercises = x.Where(x => x.Item2 != null).Select(x => x.Item2),
                        IsPublic = x.Key.IsPublic

                    });
        }

        private Task<IEnumerable<(WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)>> GetRawWorkoutPlansFromDatabase(string username)
        {
            return _sqlConnection.QueryAsync<WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO, (WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)>(
                    "[Workout].[sp_WorkoutPlan_GetUserWorkoutPlans]",
                     (workout, exercise) =>
                     {
                         return (workout, exercise);
                     },
                     new { username },
                     commandType: CommandType.StoredProcedure);
        }

    }
}
