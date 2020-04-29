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
    public class GetAllWorkoutSchedulesQueryHandler : IQueryHandler<GetAllWorkoutSchedulesQuery, IDictionary<string, IEnumerable<WorkoutScheduleDTO>>>
    {
        private readonly SqlConnection _sqlConnection;
        public GetAllWorkoutSchedulesQueryHandler(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<IDictionary<string, IEnumerable<WorkoutScheduleDTO>>> Handle(GetAllWorkoutSchedulesQuery query, CancellationToken cancellationToken)
        {
            var schedules = await _sqlConnection.QueryAsync<WorkoutSchedule>("[Workout].[sp_WorkoutSchedules_GetAll]", commandType: CommandType.StoredProcedure);
            return schedules.GroupBy(x => x.Username).ToDictionary(x => x.Key, x => x.Select(y => new WorkoutScheduleDTO
            {
                ExternalId = y.ExternalId,
                Recurrence = y.Recurrence,
                RecurringTimes = y.RecurringTimes,
                WorkoutPlanExternalId = y.WorkoutPlanExternalId,
                FirstDate = y.FirstDate
            }));

        }
        private struct WorkoutSchedule
        {
            public string Username { get; set; }
            public Guid ExternalId { get; set; }
            public int? Recurrence { get; set; }
            public DateTime FirstDate { get; set; }
            public int? RecurringTimes { get; set; }
            public Guid WorkoutPlanExternalId { get; set; }
        }
    }
}
