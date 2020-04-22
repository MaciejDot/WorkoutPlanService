﻿using SimpleCQRS.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Database;
using WorkoutPlanService.DataAccessPoint.Database.Command;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public class UpdateWorkoutPlanJob : IUpdateWorkoutPlanJob
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public UpdateWorkoutPlanJob(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Run(string username, string oldWorkoutName, WorkoutPlanPersistanceDTO workoutPlan)
        {
            await _commandDispatcher.Dispatch(new UpdateWorkoutPlanCommand
            {
                Username = username,
                OldWorkoutName = oldWorkoutName,
                WorkoutPlan = workoutPlan
            }, default);
        }
    }
}
