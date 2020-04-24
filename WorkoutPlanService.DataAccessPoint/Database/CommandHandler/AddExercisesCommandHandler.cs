using Dapper;
using SimpleCQRS.Command;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.DatetimeService;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public sealed class AddExercisesCommandHandler : ICommandHandler<AddExercisesCommand>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDateTimeService _dateTimeService;

        public AddExercisesCommandHandler(SqlConnection sqlConnection, IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _sqlConnection = sqlConnection;
        }
        public Task Handle(AddExercisesCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_Exercise_Add]", command.Exercises.Select(x=>new
            {
                Name = x.Name,
                ExerciseId = x.Id,
                IsActive = true,
                Created = _dateTimeService.GetCurrentDate()
            }), commandType: CommandType.StoredProcedure);
        }
    }
}
