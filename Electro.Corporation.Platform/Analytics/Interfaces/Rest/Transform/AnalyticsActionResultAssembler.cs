using Electro.Corporation.Platform.Analytics.Domain.Model;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Transform;

public static class AnalyticsActionResultAssembler
{
  private static int ToStatusCodeFromAnalyticsError(AnalyticsError error)
  {
    return error switch
    {
      AnalyticsError.ConsumptionNotFound => StatusCodes.Status404NotFound,
      AnalyticsError.AlertNotFound => StatusCodes.Status404NotFound,
      AnalyticsError.ReportNotFound => StatusCodes.Status404NotFound,
      AnalyticsError.PropertyNotFound => StatusCodes.Status404NotFound,
      AnalyticsError.OperationCancelled => StatusCodes.Status409Conflict,
      AnalyticsError.DatabaseError => StatusCodes.Status500InternalServerError,
      AnalyticsError.InternalServerError => StatusCodes.Status500InternalServerError,
      _ => StatusCodes.Status400BadRequest
    };
  }

  public static IActionResult ToActionResultFromConsumptionResult(
    ControllerBase controller,
    Result<Consumption> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Consumption, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromAlertResult(
    ControllerBase controller,
    Result<Alert> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Alert, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromReportResult(
    ControllerBase controller,
    Result<Report> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Report, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromPropertyMetricsResult(
    ControllerBase controller,
    Result<PropertyMetrics> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<PropertyMetrics, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromDeleteResult(
    ControllerBase controller,
    Result result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction();

    var statusCode = ToStatusCodeFromAnalyticsError((AnalyticsError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromGetConsumptionByIdResult(
    ControllerBase controller,
    Consumption? consumption,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Consumption, IActionResult> successAction)
  {
    if (consumption is null)
      return problemDetailsFactory.CreateProblemDetails(
        controller,
        ToStatusCodeFromAnalyticsError(AnalyticsError.ConsumptionNotFound),
        AnalyticsError.ConsumptionNotFound,
        errorLocalizer[nameof(AnalyticsError.ConsumptionNotFound)]
      );
    return successAction(consumption);
  }

  public static IActionResult ToActionResultFromGetReportByIdResult(
    ControllerBase controller,
    Report? report,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Report, IActionResult> successAction)
  {
    if (report is null)
      return problemDetailsFactory.CreateProblemDetails(
        controller,
        ToStatusCodeFromAnalyticsError(AnalyticsError.ReportNotFound),
        AnalyticsError.ReportNotFound,
        errorLocalizer[nameof(AnalyticsError.ReportNotFound)]
      );
    return successAction(report);
  }
}
