using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DatetimeService;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.GuidService;

namespace WorkoutPlanService.DataAccessPoint.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IGuidProvider _guidProvider;
        public readonly IDateTimeService _dateTimeService;

        public DatabaseService(SqlConnection sqlConnection, IGuidProvider guidProvider, IDateTimeService dateTimeService)
        {
            _sqlConnection = sqlConnection;
            _guidProvider = guidProvider;
            _dateTimeService = dateTimeService;
        }

        public async Task AddExercise(ExercisePersistanceDTO exercisePersistanceDTO) 
        {
            _sqlConnection.Open();
            await _sqlConnection.ExecuteAsync("[Workout].[sp_Exercise_Add]", new
            {
                Name = exercisePersistanceDTO.Name,
                ExerciseId = exercisePersistanceDTO.Id,
                IsActive = true,
                Created = _dateTimeService.GetCurrentDate()
            }, commandType: CommandType.StoredProcedure);
            _sqlConnection.Close();
        }

        public Task DeleteExercise(ExercisePersistanceDTO exercisePersistanceDTO) 
        {
            _sqlConnection.Open();
            return _sqlConnection.ExecuteAsync("[Workout].[sp_Exercise_Add]", new
            {
                Name = exercisePersistanceDTO.Name,
                ExerciseId = exercisePersistanceDTO.Id,
                IsActive = false,
                Created = _dateTimeService.GetCurrentDate()
            }, commandType: CommandType.StoredProcedure);
        }

        public Task<UserPersistanceDTO> GetUser(string username)
        {
            return _sqlConnection.QueryFirstOrDefaultAsync<UserPersistanceDTO>("[Security].[sp_User_Get]",
                new { Name = username },
                commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<UserPersistanceDTO>> GetUsers()
        {
            return _sqlConnection.QueryAsync<UserPersistanceDTO>(
                "[Security].[sp_User_GetAll]",
               commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<ExercisePersistanceDTO>> GetExercises()
        {
            return _sqlConnection.QueryAsync<ExercisePersistanceDTO>(
                "[Workout].[sp_Exercise_GetAll]", 
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddUser(UserPersistanceDTO userPersistanceDTO)
        {
            await _sqlConnection.ExecuteAsync(
                "[Security].[sp_User_Add]",
                new
                {
                    Id = _guidProvider.GetGuid(),
                    userPersistanceDTO.Name,
                    Created = _dateTimeService.GetCurrentDate()
                }
                ,
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task UpdateWorkoutPlan(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            if (oldWorkoutName != workoutPlanPersistanceDTO.Name)
            {
                await DeleteWorkoutPlan(username, oldWorkoutName, _dateTimeService.GetCurrentDate());
            }
            await AddWorkoutPlan(username, workoutPlanPersistanceDTO);
        }

        public Task DeleteWorkoutPlan(string username, string workoutName, DateTime deactivationDate)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutPlan_Delete]", new 
            { 
              Username = username, 
              WorkoutName = workoutName, 
              Created = deactivationDate,
              WorkouPlanVersionId= _guidProvider.GetGuid()
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task AddWorkoutPlan(string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            var workouPlanVersionId = _guidProvider.GetGuid();
            await SaveWorkout(workouPlanVersionId, username, workoutPlanPersistanceDTO);
            await SaveExercises(workouPlanVersionId, workoutPlanPersistanceDTO.Exercises);
        }

        private Task SaveWorkout(Guid workouPlanVersionId, string username, WorkoutPlanPersistanceDTO workoutPlanPersistanceDTO)
        {
            return _sqlConnection.ExecuteAsync("[Workout].[sp_WorkoutPlan_Add]", new
            {
                WorkoutName = workoutPlanPersistanceDTO.Name,
                workoutPlanPersistanceDTO.IsPublic,
                workoutPlanPersistanceDTO.Created,
                workoutPlanPersistanceDTO.Description,
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

        public async Task<IEnumerable<WorkoutPlanPersistanceDTO>> GetWoroutPlansAsync(string username)
        {
            var rawPlans = await GetRawWorkoutPlansFromDatabase(username);
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

        public Task<IEnumerable<UserPersistanceDTO>> GetAllUsers()
        {
            return _sqlConnection.QueryAsync<UserPersistanceDTO>("[Security].[sp_User_GetAll]", commandType: CommandType.StoredProcedure);
        }

        public async Task<IDictionary<string, IEnumerable<WorkoutPlanPersistanceDTO>>> GetAllWorkoutPlans()
        {
            var rawWorkouts = await GetAllRawWorkoutPlansFromDatabase();
            return rawWorkouts
                .GroupBy(x => x.Item1.Username)
                .ToDictionary(x => x.Key, x => FormatFetchedWorkouts(x.Select(y => (y.Item2, y.Item3))));
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
