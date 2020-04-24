using Dapper;
using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.DatetimeService;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public class DeleteExercisesCommandHandler : ICommandHandler<DeleteExercisesCommand>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDateTimeService _dateTimeService;

        public DeleteExercisesCommandHandler(SqlConnection sqlConnection, IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _sqlConnection = sqlConnection;
        }
        public Task Handle(DeleteExercisesCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_Exercise_Add]", command.Ids.Select( x=> new
            {
                Name = "deleted",
                ExerciseId = x,
                IsActive = false,
                Created = _dateTimeService.GetCurrentDate()
            }), commandType: CommandType.StoredProcedure);
        }
    }
}
