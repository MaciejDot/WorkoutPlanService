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
    public sealed class GetAllWorkoutPlansQueryHandler : IQueryHandler<GetAllWorkoutPlansQuery, IDictionary<string, IEnumerable<WorkoutPlanPersistanceDTO>>>
    {
        private readonly SqlConnection _sqlConnection;
        public GetAllWorkoutPlansQueryHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<IDictionary<string, IEnumerable<WorkoutPlanPersistanceDTO>>> Handle(GetAllWorkoutPlansQuery query, CancellationToken cancellationToken)
        {
            var rawWorkouts = await GetAllRawWorkoutPlansFromDatabase();
            return rawWorkouts
                .GroupBy(x => x.Item1.Username)
                .ToDictionary(x => x.Key, x => FormatFetchedWorkouts(x.Select(y => (y.Item2, y.Item3))));
        }

        private IEnumerable<WorkoutPlanPersistanceDTO> FormatFetchedWorkouts(IEnumerable<(WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)> rawWorkoutPlans)
        {
            return rawWorkoutPlans
                    .GroupBy(x => new
                    {
                        x.Item1.Name,
                        x.Item1.Created,
                        x.Item1.Description,
                        x.Item1.IsPublic,
                        x.Item1.ExternalId
                    })
                    .Select(x => new WorkoutPlanPersistanceDTO
                    {
                        Name = x.Key.Name,
                        Created = x.Key.Created,
                        Description = x.Key.Description,
                        Exercises = x.Where(x => x.Item2 != null).Select(x => x.Item2),
                        IsPublic = x.Key.IsPublic,
                        ExternalId = x.Key.ExternalId
                    });
        }

        private Task<IEnumerable<(User, WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)>> GetAllRawWorkoutPlansFromDatabase()
        {
            return _sqlConnection.QueryAsync<User, WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO, (User, WorkoutPlanPersistanceDTO, ExerciseExecutionPersistanceDTO)>(
                    "[Workout].[sp_WorkoutPlan_GetAll]",
                     (user, workout, exercise) =>
                     {
                         return (user, workout, exercise);
                     },
                     commandType: CommandType.StoredProcedure);
        }

        private class User
        {
            public string Username { get; set; }
        }

    }
}
