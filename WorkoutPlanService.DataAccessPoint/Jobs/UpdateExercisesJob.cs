using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Configuration;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    //do przerobienie nasluchuje zmian z mb 
    public class UpdateExercisesJob : IUpdateExercisesJob
    {
        private readonly IDatabaseService _databaseService;
        private readonly IExerciseCacheService _exerciseCacheService;
        private readonly IRestClient _restClient;
        private readonly ExerciseServiceOptions _exerciseServiceOptions;
        private readonly IBackgroundJobClientService _backgroundJobClientService;

        public UpdateExercisesJob(
            IDatabaseService databaseService,
            IExerciseCacheService exerciseCacheService,
            IRestClient restClient,
            IOptions<ExerciseServiceOptions> exerciseServiceOptions,
            IBackgroundJobClientService backgroundJobClientService
            )
        {
            _exerciseCacheService = exerciseCacheService;
            _restClient = restClient;
            _exerciseServiceOptions = exerciseServiceOptions.Value;
            _databaseService = databaseService;
            _backgroundJobClientService = backgroundJobClientService;
        }

        public async Task Run()
        {
            try
            {
                var databaseExercises = await _databaseService.GetExercises();
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
                    exercises
                        .Where(x => databaseExercises.Any(y => y == x))
                        .AsParallel()
                        .ForAll(async x =>
                        {
                            if (databaseExercises.Any(y => y.Id == x.Id))
                            {
                                await _databaseService.UpdateExercise(x);
                            }
                            else
                            {
                                await _databaseService.AddExercise(x);
                            }
                        });
                }
            }
            finally 
            {
                _backgroundJobClientService.Schedule<IUpdateExercisesJob>(x => x.Run(), TimeSpan.FromMinutes(60));
            }

        }
    }
}
