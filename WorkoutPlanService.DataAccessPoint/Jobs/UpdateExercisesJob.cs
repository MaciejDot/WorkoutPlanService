using Microsoft.Extensions.Options;
using RestSharp;
using SimpleCQRS.Command;
using SimpleCQRS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Configuration;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.Database.Query;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    //do przerobienie nasluchuje zmian z mb 
    public sealed class UpdateExercisesJob : IUpdateExercisesJob
    {
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandDispatcher _commandDispatcher; 
        private readonly IExerciseCacheService _exerciseCacheService;
        private readonly IRestClient _restClient;
        private readonly ExerciseServiceOptions _exerciseServiceOptions;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public UpdateExercisesJob(
            IQueryProcessor queryProcessor,
            IExerciseCacheService exerciseCacheService,
            IRestClient restClient,
            IOptions<ExerciseServiceOptions> exerciseServiceOptions,
            IBackgroundJobClientService backgroundJobClientService,
            ICommandDispatcher commandDispatcher
            )
        {
            _exerciseCacheService = exerciseCacheService;
            _restClient = restClient;
            _exerciseServiceOptions = exerciseServiceOptions.Value;
            _queryProcessor = queryProcessor;
            _backgroundJobClientService = backgroundJobClientService;
            _commandDispatcher = commandDispatcher;
        }

        public async Task Run()
        {
            try
            {
                var databaseExercises = await _queryProcessor.Process(new GetExercisesQuery(), default);
                var request = new RestRequest(_exerciseServiceOptions.ExerciseServiceGetExercisesEndpoint, Method.GET);
                var response = await _restClient.ExecuteAsync<IEnumerable<ExercisePersistanceDTO>>(request);
                if (!response.IsSuccessful)
                {
                    _exerciseCacheService.PutExercises(databaseExercises);
                    throw new Exception("Unsuccesful request");
                }
                var exercises = response.Data;
                if (!exercises.OrderBy(x => x.Id).SequenceEqual(databaseExercises.OrderBy(x => x.Id)))
                {
                    _exerciseCacheService.PutExercises(exercises);
                    if (exercises
                        .Where(x => !databaseExercises.Any(y => y.Id == x.Id && x.Name == y.Name)).Any())
                    {
                        await _commandDispatcher.Dispatch(new AddExercisesCommand
                        {
                            Exercises =
                        exercises
                            .Where(x => !databaseExercises.Any(y => y.Id == x.Id && x.Name == y.Name))
                        }, default);
                    }

                    if (databaseExercises
                        .Where(x => !exercises.Any(y => y.Id == x.Id)).Any())
                    {
                        await _commandDispatcher.Dispatch(new DeleteExercisesCommand
                        {
                            Ids =
                        databaseExercises
                            .Where(x => !exercises.Any(y => y.Id == x.Id))
                            .Select(x => x.Id)
                        }, default);
                    }
                }
            }
            catch
            { 
                //
            }
            finally
            {
                _backgroundJobClientService.Schedule<IUpdateExercisesJob>(x => x.Run(), TimeSpan.FromMinutes(60));
            }

        }
    }
}
