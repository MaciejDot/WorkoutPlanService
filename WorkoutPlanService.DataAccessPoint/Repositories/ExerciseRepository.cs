﻿using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Cache;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.DataAccessPoint.Hangfire;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly IDatabaseService _databaseService;
        private readonly IBackgroundJobClientService _backgroundJobClientService;
        private readonly IExerciseCacheService _exerciseCacheService;

        public ExerciseRepository(
            IDatabaseService databaseService,
            IBackgroundJobClientService backgroundJobClientService,
            IExerciseCacheService exerciseCacheService)
        {
            _databaseService = databaseService;
            _backgroundJobClientService = backgroundJobClientService;
            _exerciseCacheService = exerciseCacheService;
        }

        public Task<IEnumerable<ExercisePersistanceDTO>> GetAllExercisesAsync()
        {
            var cachedItem = _exerciseCacheService.GetAllExercises();
            return HandleReturnedCacheItem(cachedItem);
        }

        private async Task<IEnumerable<ExercisePersistanceDTO>> HandleReturnedCacheItem(CacheItem<IEnumerable<ExercisePersistanceDTO>> cacheItem)
        {
            if (cacheItem == null)
            {
                return await HandleNullCacheItem(cacheItem);
            }
            else
            {
                HandleCloseToExpiration(cacheItem);
                return cacheItem.Value;
            }
        }

        private async Task<IEnumerable<ExercisePersistanceDTO>> HandleNullCacheItem(CacheItem<IEnumerable<ExercisePersistanceDTO>> cacheItem)
        {
            var exercisesFromDatabase = await _databaseService.GetExercises();
            _exerciseCacheService.PutExercises(exercisesFromDatabase);
            return exercisesFromDatabase;
        }

        private void HandleCloseToExpiration(CacheItem<IEnumerable<ExercisePersistanceDTO>> cacheItem)
        {
            if (cacheItem.ExpirationTimeout < TimeSpan.FromMinutes(30))
            {
             //   ScheduleUpdateJob();
            }
        }

        private void ScheduleUpdateJob()
        {
           // _backgroundJobClientService.Enqueue<IUpdateExercisesJob>(x => x.Run(default));
        }
    }
}