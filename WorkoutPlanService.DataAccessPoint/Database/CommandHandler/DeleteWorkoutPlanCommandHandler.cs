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
using WorkoutPlanService.DataAccessPoint.DatetimeService;
using WorkoutPlanService.DataAccessPoint.GuidService;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public sealed class DeleteWorkoutPlanCommandHandler : ICommandHandler<DeleteWorkoutPlanCommand>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDateTimeService _dateTimeService;
        private readonly IGuidProvider _guidProvider;

        public DeleteWorkoutPlanCommandHandler(SqlConnection sqlConnection, IDateTimeService dateTimeService, IGuidProvider guidProvider)
        {
            _dateTimeService = dateTimeService;
            _sqlConnection = sqlConnection;
            _guidProvider = guidProvider;
        }

        public Task Handle(DeleteWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutPlan_Delete]", new
            {
                Username = command.Username,
                WorkoutName = "--deleted--",
                ExternalId = command.ExternalId,
                Created = _dateTimeService.GetCurrentDate(),
                WorkouPlanVersionId = _guidProvider.GetGuid()
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
