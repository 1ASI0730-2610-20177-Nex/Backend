using System.Net.Mime;
using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Simulation session management endpoints.")]
public class SimulationSessionsController(
    ISimulationSessionCommandService simulationSessionCommandService,
    ISimulationSessionQueryService simulationSessionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(Summary = "Start simulation session", OperationId = "StartSimulationSession")]
    [SwaggerResponse(StatusCodes.Status201Created, "Session started", typeof(SimulationSessionResource))]
    public async Task<IActionResult> StartSimulationSession([FromBody] CreateSimulationSessionResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSimulationSessionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await simulationSessionCommandService.Handle(command, cancellationToken);
        return DevicesActionResultAssembler.ToActionResultFromSessionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdSession => CreatedAtAction(
                nameof(GetSimulationSessionById),
                new { sessionId = createdSession.Id },
                SimulationSessionResourceFromEntityAssembler.ToResourceFromEntity(createdSession, []))
        );
    }

    [HttpPost("{sessionId:int}/actions")]
    [SwaggerOperation(Summary = "Register simulation action", OperationId = "RegisterSimulationAction")]
    [SwaggerResponse(StatusCodes.Status201Created, "Action registered", typeof(SimulationActionResource))]
    public async Task<IActionResult> RegisterSimulationAction(int sessionId,
        [FromBody] CreateSimulationActionResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterSimulationActionCommandFromResourceAssembler.ToCommandFromResource(sessionId, resource);
        var result = await simulationSessionCommandService.Handle(command, cancellationToken);
        return DevicesActionResultAssembler.ToActionResultFromActionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            createdAction => CreatedAtAction(
                nameof(GetSimulationSessionById),
                new { sessionId = createdAction.SessionId },
                SimulationActionResourceFromEntityAssembler.ToResourceFromEntity(createdAction))
        );
    }

    [HttpPost("{sessionId:int}/end")]
    [SwaggerOperation(Summary = "End simulation session", OperationId = "EndSimulationSession")]
    [SwaggerResponse(StatusCodes.Status200OK, "Session ended", typeof(SimulationSessionResource))]
    public async Task<IActionResult> EndSimulationSession(int sessionId, CancellationToken cancellationToken)
    {
        var result = await simulationSessionCommandService.Handle(new EndSimulationSessionCommand(sessionId),
            cancellationToken);
        if (!result.IsSuccess)
            return DevicesActionResultAssembler.ToActionResultFromSessionResult(
                this,
                result,
                _errorLocalizer,
                _problemDetailsFactory,
                _ => Ok());

        var (_, actions) = await simulationSessionQueryService.Handle(
            new GetSimulationSessionByIdQuery(result.Value!.Id), cancellationToken);
        return Ok(SimulationSessionResourceFromEntityAssembler.ToResourceFromEntity(result.Value!, actions));
    }

    [HttpGet("{sessionId:int}")]
    [SwaggerOperation(Summary = "Get simulation session by id", OperationId = "GetSimulationSessionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Session found", typeof(SimulationSessionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Session not found")]
    public async Task<IActionResult> GetSimulationSessionById(int sessionId, CancellationToken cancellationToken)
    {
        var (session, actions) =
            await simulationSessionQueryService.Handle(new GetSimulationSessionByIdQuery(sessionId),
                cancellationToken);
        return DevicesActionResultAssembler.ToActionResultFromGetSessionResult(
            this,
            session,
            actions,
            _errorLocalizer,
            _problemDetailsFactory,
            (foundSession, foundActions) =>
                Ok(SimulationSessionResourceFromEntityAssembler.ToResourceFromEntity(foundSession, foundActions))
        );
    }

    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get active simulation session for user", OperationId = "GetActiveSimulationSession")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active session found", typeof(SimulationSessionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Active session not found")]
    public async Task<IActionResult> GetActiveSimulationSession([FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        var (session, actions) =
            await simulationSessionQueryService.Handle(new GetActiveSimulationSessionByUserIdQuery(userId),
                cancellationToken);
        return DevicesActionResultAssembler.ToActionResultFromGetActiveSessionResult(
            this,
            session,
            actions,
            _errorLocalizer,
            _problemDetailsFactory,
            (foundSession, foundActions) =>
                Ok(SimulationSessionResourceFromEntityAssembler.ToResourceFromEntity(foundSession, foundActions))
        );
    }
}
