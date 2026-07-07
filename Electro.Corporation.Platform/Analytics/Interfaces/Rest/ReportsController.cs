using System.Net.Mime;
using Electro.Corporation.Platform.Analytics.Application.CommandServices;
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
[SwaggerTag("Report management endpoints.")]
public class ReportsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
  private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
  private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

  [HttpPost]
  [SwaggerOperation(Summary = "Create report", OperationId = "CreateReport")]
  [SwaggerResponse(StatusCodes.Status201Created, "Report created", typeof(ReportResource))]
  public async Task<IActionResult> CreateReport([FromBody] CreateReportResource resource,
    CancellationToken cancellationToken)
  {
    var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
    var result = await reportCommandService.Handle(command, cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromReportResult(
      this,
      result,
      _errorLocalizer,
      _problemDetailsFactory,
      createdReport => CreatedAtAction(nameof(GetReportById), new { reportId = createdReport.Id },
        ReportResourceFromEntityAssembler.ToResourceFromEntity(createdReport))
    );
  }

  [HttpGet]
  [SwaggerOperation(Summary = "Get reports by property id", OperationId = "GetReportsByPropertyId")]
  [SwaggerResponse(StatusCodes.Status200OK, "List of reports", typeof(IEnumerable<ReportResource>))]
  public async Task<IActionResult> GetReportsByPropertyId([FromQuery] int propertyId,
    CancellationToken cancellationToken)
  {
    var reports = await reportQueryService.Handle(new GetReportsByPropertyIdQuery(propertyId), cancellationToken);
    return Ok(reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity));
  }

  [HttpGet("{reportId:int}")]
  [SwaggerOperation(Summary = "Get report by id", OperationId = "GetReportById")]
  [SwaggerResponse(StatusCodes.Status200OK, "Report found", typeof(ReportResource))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Report not found")]
  public async Task<IActionResult> GetReportById(int reportId, CancellationToken cancellationToken)
  {
    var report = await reportQueryService.Handle(new GetReportByIdQuery(reportId), cancellationToken);
    return AnalyticsActionResultAssembler.ToActionResultFromGetReportByIdResult(
      this,
      report,
      _errorLocalizer,
      _problemDetailsFactory,
      foundReport => Ok(ReportResourceFromEntityAssembler.ToResourceFromEntity(foundReport))
    );
  }
}
