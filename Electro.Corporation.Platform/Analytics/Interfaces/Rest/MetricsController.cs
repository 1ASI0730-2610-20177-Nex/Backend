using System.Net.Mime;
using Electro.Corporation.Platform.Analytics.Application.QueryServices;
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
[SwaggerTag("Property metrics endpoints.")]
public class MetricsController(
    IMetricsQueryService metricsQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpGet]
  [SwaggerOperation(Summary = "Get property metrics", OperationId = "GetPropertyMetrics")]
  [SwaggerResponse(StatusCodes.Status200OK, "Property metrics", typeof(PropertyMetricsResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Property not found")]
  public async Task<IActionResult> GetPropertyMetrics([FromQuery] int propertyId,
    CancellationToken cancellationToken)
  {
    var result = await metricsQueryService.Handle(new GetPropertyMetricsQuery(propertyId), cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromPropertyMetricsResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      metrics => Ok(PropertyMetricsResourceFromEntityAssembler.ToResourceFromEntity(metrics))
    );
  }
}
