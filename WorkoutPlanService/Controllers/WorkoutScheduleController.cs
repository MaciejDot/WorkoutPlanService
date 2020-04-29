using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Query;
using WorkoutPlanService.Models;

namespace WorkoutPlanService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkoutScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkoutScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkoutScheduleDTO>>> Get(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetWorkoutSchedulesQuery { Username = User.Identity.Name }, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WorkoutScheduleIdentityDTO>> Post(WorkoutSchedulePostModel model, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new AddWorkoutScheduleCommand {
                WorkoutPlanExternalId = model.WorkoutPlanExternalId,
                RecurringTimes = model.RecurringTimes,
                Recurrence = model.Recurrence,
                FirstDate = model.FirstDate,
                Username = User.Identity.Name }
            , cancellationToken));
        }

        [HttpDelete("{externalId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid externalId, CancellationToken cancellationToken) 
        {
            await _mediator.Send(new DeleteWorkoutScheduleCommand { Username = User.Identity.Name, ExternalId = externalId }, cancellationToken);
            return Ok();
        }

        [HttpPatch("{externalId}")]
        [Authorize]
        public async Task<IActionResult> Patch(Guid externalId, [FromBody] WorkoutSchedulePatchModel model, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateWorkoutScheduleCommand { 
                Username = User.Identity.Name, 
                ExternalId = externalId,
                WorkoutPlanExternalId = model.WorkoutPlanExternalId,
                FirstDate = model.FirstDate,
                Recurrence = model.Recurrence,
                RecurringTimes = model.RecurringTimes
            }, cancellationToken);
            return Ok();
        }
    }
}