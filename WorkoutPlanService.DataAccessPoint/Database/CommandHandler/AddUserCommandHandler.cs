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

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public sealed class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDateTimeService _dateTimeService;

        public AddUserCommandHandler(SqlConnection sqlConnection, IDateTimeService dateTimeService) 
        {
            _dateTimeService = dateTimeService;
            _sqlConnection = sqlConnection;
        }
        
        public Task Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            return _sqlConnection.ExecuteAsync(
                "[Security].[sp_User_Add]",
                new
                {
                    command.Name,
                    Created = _dateTimeService.GetCurrentDate()
                }
                ,
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
