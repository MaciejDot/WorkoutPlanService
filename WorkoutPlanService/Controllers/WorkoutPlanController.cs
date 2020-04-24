using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkoutPlanService.DataAccessPoint.DTO;
using WorkoutPlanService.Domain.Command;
using WorkoutPlanService.Domain.DTO;
using WorkoutPlanService.Domain.Models;
using WorkoutPlanService.Domain.Query;
using WorkoutPlanService.Models;

namespace WorkoutPlanService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class WorkoutPlanController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WorkoutPlanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkoutPlanThumbnailDTO>>> Get(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetAllUserWorkoutPlansQuery { Username = User.Identity.Name }, cancellationToken));
        }

        [HttpGet("{username}/{workoutName}")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkoutPlanPersistanceDTO>> Get(string username, string workoutName, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetSpecificWorkoutQuery {
                IssuerName = User?.Identity?.Name,
                OwnerName = username,
                WorkoutName = workoutName
            }, cancellationToken));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]WorkoutPlanPostModel model, CancellationToken cancellationToken) 
        {
            await _mediator.Send(new CreateWorkoutPlanCommand {
                Name = model.Name,
                Description = model.Description,
                Username = User.Identity.Name,
                IsPublic = model.IsPublic,
                Exercises = model.Exercises.Select(x=> new ExercisePlanCommandModel {
                    Break = x.Break,
                    Description = x.Description,
                    ExerciseId = x.ExerciseId,
                    ExerciseName = x.ExerciseName,
                    MaxAdditionalKgs = x.MaxAdditionalKgs,
                    MinAdditionalKgs = x.MinAdditionalKgs,
                    MaxReps = x.MaxReps,
                    MinReps = x.MinReps,
                    Order = x.Order,
                    Series = x.Series
                })
            }, cancellationToken);
            return Ok();
        }

        [HttpPatch("{workoutName}")]
        [Authorize]
        public async Task<IActionResult> Patch(string workoutName, [FromBody]WorkoutPlanPatchModel model, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateWorkoutPlanCommand
            {
                OldName = workoutName,
                NewName = model.Name,
                Description = model.Description,
                Username = User.Identity.Name,
                IsPublic = model.IsPublic,
                Exercises = model.Exercises.Select(x => new ExercisePlanCommandModel
                {
                    Break = x.Break,
                    Description = x.Description,
                    ExerciseId = x.ExerciseId,
                    ExerciseName = x.ExerciseName,
                    MaxAdditionalKgs = x.MaxAdditionalKgs,
                    MinAdditionalKgs = x.MinAdditionalKgs,
                    MaxReps = x.MaxReps,
                    MinReps = x.MinReps,
                    Order = x.Order,
                    Series = x.Series
                })
            }, cancellationToken);
            return Ok();
        }

        [HttpDelete("{workoutName}")]
        [Authorize]
        public async Task<ActionResult<WorkoutPlanPersistanceDTO>> Delete(string workoutName, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new DeleteWorkoutPlanCommand
            {
                Username = User.Identity.Name,
                WorkoutName = workoutName
            }, cancellationToken));
        }
    }
}
