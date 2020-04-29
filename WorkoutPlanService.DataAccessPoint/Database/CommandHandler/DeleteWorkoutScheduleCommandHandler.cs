using Dapper;
using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public class DeleteWorkoutScheduleCommandHandler : ICommandHandler<DeleteWorkoutScheduleCommand>
    {
        private readonly SqlConnection _sqlConnection;
        public DeleteWorkoutScheduleCommandHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }
        public Task Handle(DeleteWorkoutScheduleCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutSchedule_Add]", new
            {
                command.Created,
                command.ExternalId,
                FirstDate = DateTime.Parse("2000-10-20"),
                Recurrence = -1,
                RecurringTimes = -1,
                command.WorkoutPlanExternalId,
                IsActive = false,
            },
                commandType: CommandType.StoredProcedure);
        }
    }
}
