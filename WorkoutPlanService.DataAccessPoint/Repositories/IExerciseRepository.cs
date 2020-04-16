using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.DTO;

namespace WorkoutPlanService.DataAccessPoint.Repositories
{
    public interface IExerciseRepository
    {
        public Task<IEnumerable<ExercisePersistanceDTO>> GetAllExercisesAsync();
    }
}
