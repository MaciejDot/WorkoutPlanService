using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkoutPlanService.DataAccessPoint.Repositories;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Query;

namespace WorkoutPlanService.Domain.QueryHandler
{
    public class GetWorkoutSchedulesQueryHandler : IRequestHandler<GetWorkoutSchedulesQuery, IEnumerable<WorkoutScheduleDTO>>
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IWorkoutSchedulesRepository _workoutSchedulesRepository;
        public GetWorkoutSchedulesQueryHandler(IWorkoutPlanRepository workoutPlanRepository,
            IWorkoutSchedulesRepository workoutSchedulesRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
            _workoutSchedulesRepository = workoutSchedulesRepository;
        }
        public async Task<IEnumerable<WorkoutScheduleDTO>> Handle(GetWorkoutSchedulesQuery request, CancellationToken cancellationToken)
        {

            var schedulesTask = _workoutSchedulesRepository.GetWorkoutSchedules(request.Username);
            var plansTask = _workoutPlanRepository.GetAllUserWorkutPlansAsync(request.Username);
            await Task.WhenAll(schedulesTask, plansTask);
            var plans = plansTask.Result;
            var schedules = schedulesTask.Result;
            return schedulesTask.Result
                .Where(x => plans.Any(y => y.ExternalId == x.WorkoutPlanExternalId))
                .Select(x => new WorkoutScheduleDTO
                {
                    WorkoutPlanName = plans.First(y => y.ExternalId == x.WorkoutPlanExternalId).Name,
                    ExternalId = x.ExternalId,
                    FirstDate = x.FirstDate,
                    Recurrance = x.Recurrence,
                    ReccuringTimes = x.RecurringTimes,
                    WorkoutPlanExternalId = x.WorkoutPlanExternalId
                });
        }
    }
}
