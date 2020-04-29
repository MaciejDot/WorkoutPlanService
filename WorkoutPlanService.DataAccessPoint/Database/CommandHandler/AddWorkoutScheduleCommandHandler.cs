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
    public class AddWorkoutScheduleCommandHandler : ICommandHandler<AddWorkoutScheduleCommand>
    {
        private readonly SqlConnection _sqlConnection;
        public AddWorkoutScheduleCommandHandler(SqlConnection sqlConnection) 
        {
            _sqlConnection = sqlConnection;
        }
        public Task Handle(AddWorkoutScheduleCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutSchedule_Add]", new
            {
                command.Created,
                command.ExternalId,
                command.FirstDate,
                command.Recurrence,
                command.RecurringTimes,
                command.WorkoutPlanExternalId,
                IsActive = true,
            },
                commandType: CommandType.StoredProcedure);
        }
    }
}
