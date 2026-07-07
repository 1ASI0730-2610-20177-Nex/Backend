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
[SwaggerTag("Alert management endpoints.")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpGet]
  [SwaggerOperation(Summary = "Get alerts by user id", OperationId = "GetAlertsByUserId")]
  [SwaggerResponse(StatusCodes.Status200OK, "List of alerts", typeof(IEnumerable<AlertResource>))]
  public async Task<IActionResult> GetAlertsByUserId([FromQuery] int userId,
    CancellationToken cancellationToken)
  {
    var alerts = await alertQueryService.Handle(new GetAlertsByUserIdQuery(userId), cancellationToken);
    return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
  }

  [HttpPut("{alertId:int}/read")]
  [SwaggerOperation(Summary = "Mark alert as read", OperationId = "MarkAlertAsRead")]
  [SwaggerResponse(StatusCodes.Status200OK, "Alert marked as read", typeof(AlertResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Alert not found")]
  public async Task<IActionResult> MarkAlertAsRead(int alertId, CancellationToken cancellationToken)
  {
    var result = await alertCommandService.Handle(new MarkAlertAsReadCommand(alertId), cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromAlertResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      alert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(alert))
    );
  }
}
