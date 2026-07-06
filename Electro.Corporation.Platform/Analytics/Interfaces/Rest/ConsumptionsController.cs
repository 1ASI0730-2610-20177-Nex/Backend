using System.Net.Mime;
using Electro.Corporation.Platform.Analytics.Application.CommandServices;
using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Analytics.Interfaces.Rest.Transform;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Energy consumption management endpoints.")]
public class ConsumptionsController(
    IConsumptionCommandService consumptionCommandService,
    IConsumptionQueryService consumptionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpGet]
  [SwaggerOperation(Summary = "Get all consumptions", OperationId = "GetAllConsumptions")]
  [SwaggerResponse(StatusCodes.Status200OK, "List of consumptions", typeof(IEnumerable<ConsumptionResource>))]
  public async Task<IActionResult> GetAllConsumptions(CancellationToken cancellationToken)
  {
    var consumptions = await consumptionQueryService.Handle(new GetAllConsumptionsQuery(), cancellationToken);
    return Ok(consumptions.Select(ConsumptionResourceFromEntityAssembler.ToResourceFromEntity));
  }

  [HttpGet("{id:int}")]
  [SwaggerOperation(Summary = "Get consumption by id", OperationId = "GetConsumptionById")]
  [SwaggerResponse(StatusCodes.Status200OK, "Consumption found", typeof(ConsumptionResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Consumption not found")]
  public async Task<IActionResult> GetConsumptionById(int id, CancellationToken cancellationToken)
  {
    var consumption = await consumptionQueryService.Handle(new GetConsumptionByIdQuery(id), cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromGetConsumptionByIdResult(
      this,
      consumption,
      _errorLocalizer,
      _problemDetailsFactory,
      foundConsumption =>
        Ok(ConsumptionResourceFromEntityAssembler.ToResourceFromEntity(foundConsumption))
    );
  }

  [HttpPost]
  [SwaggerOperation(Summary = "Create consumption", OperationId = "CreateConsumption")]
  [SwaggerResponse(StatusCodes.Status201Created, "Consumption created", typeof(ConsumptionResource))]
  public async Task<IActionResult> CreateConsumption([FromBody] CreateConsumptionResource resource,
    CancellationToken cancellationToken)
  {
    var command = CreateConsumptionCommandFromResourceAssembler.ToCommandFromResource(resource);
    var result = await consumptionCommandService.Handle(command, cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromConsumptionResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      createdConsumption => CreatedAtAction(nameof(GetConsumptionById), new { id = createdConsumption.Id },
        ConsumptionResourceFromEntityAssembler.ToResourceFromEntity(createdConsumption))
    );
  }

  [HttpPut("{id:int}")]
  [SwaggerOperation(Summary = "Update consumption", OperationId = "UpdateConsumption")]
  [SwaggerResponse(StatusCodes.Status200OK, "Consumption updated", typeof(ConsumptionResource))]
  public async Task<IActionResult> UpdateConsumption(int id, [FromBody] UpdateConsumptionResource resource,
    CancellationToken cancellationToken)
  {
    var command = UpdateConsumptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
    var result = await consumptionCommandService.Handle(command, cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromConsumptionResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      updatedConsumption =>
        Ok(ConsumptionResourceFromEntityAssembler.ToResourceFromEntity(updatedConsumption))
    );
  }

  [HttpDelete("{id:int}")]
  [SwaggerOperation(Summary = "Delete consumption", OperationId = "DeleteConsumption")]
  [SwaggerResponse(StatusCodes.Status204NoContent, "Consumption deleted")]
  public async Task<IActionResult> DeleteConsumption(int id, CancellationToken cancellationToken)
  {
    var result = await consumptionCommandService.Handle(new DeleteConsumptionCommand(id), cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromDeleteResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      () => NoContent()
    );
  }
}
