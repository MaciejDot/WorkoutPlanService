using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanService.DataAccessPoint.Jobs
{
    public interface IUpdateExercisesJob
    {
        public Task Run();
    }
}
