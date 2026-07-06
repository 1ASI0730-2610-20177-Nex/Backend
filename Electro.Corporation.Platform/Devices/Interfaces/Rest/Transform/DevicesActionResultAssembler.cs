using Electro.Corporation.Platform.Devices.Domain.Model;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class DevicesActionResultAssembler
{
  private static int ToStatusCodeFromDevicesError(DevicesError error)
  {
    return error switch
    {
      DevicesError.HomeNotFound => StatusCodes.Status404NotFound,
      DevicesError.DeviceNotFound => StatusCodes.Status404NotFound,
      DevicesError.OperationCancelled => StatusCodes.Status409Conflict,
      DevicesError.DatabaseError => StatusCodes.Status500InternalServerError,
      DevicesError.InternalServerError => StatusCodes.Status500InternalServerError,
      _ => StatusCodes.Status400BadRequest
    };
  }

  public static IActionResult ToActionResultFromHomeResult(
    ControllerBase controller,
    Result<Home> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Home, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromDevicesError((DevicesError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromDeviceResult(
    ControllerBase controller,
    Result<Device> result,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Device, IActionResult> successAction)
  {
    if (result.IsSuccess) return successAction(result.Value!);

    var statusCode = ToStatusCodeFromDevicesError((DevicesError)result.Error!);
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

    var statusCode = ToStatusCodeFromDevicesError((DevicesError)result.Error!);
    return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
  }

  public static IActionResult ToActionResultFromGetHomeByIdResult(
    ControllerBase controller,
    Home? home,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Home, IActionResult> successAction)
  {
    if (home is null)
      return problemDetailsFactory.CreateProblemDetails(
        controller,
        ToStatusCodeFromDevicesError(DevicesError.HomeNotFound),
        DevicesError.HomeNotFound,
        errorLocalizer[nameof(DevicesError.HomeNotFound)]
      );
    return successAction(home);
  }

  public static IActionResult ToActionResultFromGetDeviceByIdResult(
    ControllerBase controller,
    Device? device,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory,
    Func<Device, IActionResult> successAction)
  {
    if (device is null)
      return problemDetailsFactory.CreateProblemDetails(
        controller,
        ToStatusCodeFromDevicesError(DevicesError.DeviceNotFound),
        DevicesError.DeviceNotFound,
        errorLocalizer[nameof(DevicesError.DeviceNotFound)]
      );
    return successAction(device);
  }
}
