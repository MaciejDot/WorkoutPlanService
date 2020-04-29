using Dapper;
using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.DatetimeService;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.GuidService;

namespace WorkoutPlanService.DataAccessPoint.Database.CommandHandler
{
    public sealed class AddWorkoutPlanCommandHandler : ICommandHandler<AddWorkoutPlanCommand>
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IGuidProvider _guidProvider;

        public AddWorkoutPlanCommandHandler(SqlConnection sqlConnection, IGuidProvider guidProvider)
        {
            _sqlConnection = sqlConnection;
            _guidProvider = guidProvider;
        }

        public async Task Handle(AddWorkoutPlanCommand command, CancellationToken cancellationToken)
        {
            var workouPlanVersionId = _guidProvider.GetGuid();
            await SaveWorkout(workouPlanVersionId, command.Username, command.WorkoutPlan);
            await SaveExercises(workouPlanVersionId, command.WorkoutPlan.Exercises);
        }

        private Task SaveWorkout(Guid workouPlanVersionId, string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutPlan_Add]", new
            {
                WorkoutName = workoutPlanPersistanceDTO.Name,
                workoutPlanPersistanceDTO.IsPublic,
                workoutPlanPersistanceDTO.Created,
                workoutPlanPersistanceDTO.Description,
                workoutPlanPersistanceDTO.ExternalId,
                Username = username,
                WorkouPlanVersionId = workouPlanVersionId,
                IsActive = true,
            },
                commandType: CommandType.StoredProcedure);
        }

        private Task SaveExercises(Guid workouPlanVersionId, IEnumerable<ExerciseExecutionPersistanceDTO> exercisePersistanceDTOs)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_ExerciseExecutionPlan_Add]",
                exercisePersistanceDTOs.Select(x => new
                {
                    WorkoutPlanVersionId = workouPlanVersionId,
                    x.MaxAdditionalKgs,
                    x.MaxReps,
                    x.MinAdditionalKgs,
                    x.MinReps,
                    x.Order,
                    x.Series,
                    x.Description,
                    x.Break,
                    x.ExerciseId
                }),
            commandType: CommandType.StoredProcedure);
        }
    }
}
