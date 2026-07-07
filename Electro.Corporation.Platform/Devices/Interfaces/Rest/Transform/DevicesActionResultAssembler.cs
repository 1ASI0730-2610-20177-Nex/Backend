using Electro.Corporation.Platform.Devices.Domain.Model;
using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class DevicesActionResultAssembler
{
    private static int ToStatusCode(DevicesError error)
    {
        return error switch
        {
            DevicesError.PropertyNotFound => StatusCodes.Status404NotFound,
            DevicesError.SpaceNotFound => StatusCodes.Status404NotFound,
            DevicesError.DeviceNotFound => StatusCodes.Status404NotFound,
            DevicesError.SessionNotFound => StatusCodes.Status404NotFound,
            DevicesError.ActiveSessionNotFound => StatusCodes.Status404NotFound,
            DevicesError.UserNotFound => StatusCodes.Status404NotFound,
            DevicesError.SessionAlreadyEnded => StatusCodes.Status409Conflict,
            DevicesError.ActiveSessionAlreadyExists => StatusCodes.Status409Conflict,
            DevicesError.OperationCancelled => StatusCodes.Status409Conflict,
            DevicesError.DatabaseError => StatusCodes.Status500InternalServerError,
            DevicesError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromPropertyResult(
        ControllerBase controller, Result<Property> result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<Property, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromSpaceResult(
        ControllerBase controller, Result<Space> result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<Space, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetPropertyByIdResult(
        ControllerBase controller, Property? property,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<Property, IActionResult> successAction)
    {
        if (property is null)
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(DevicesError.PropertyNotFound),
                DevicesError.PropertyNotFound, errorLocalizer[nameof(DevicesError.PropertyNotFound)]);
        return successAction(property);
    }

    public static IActionResult ToActionResultFromDeviceResult(
        ControllerBase controller, Result<Device> result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<Device, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromDeleteResult(
        ControllerBase controller, Result result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetDeviceByIdResult(
        ControllerBase controller, Device? device,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<Device, IActionResult> successAction)
    {
        if (device is null)
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(DevicesError.DeviceNotFound),
                DevicesError.DeviceNotFound, errorLocalizer[nameof(DevicesError.DeviceNotFound)]);
        return successAction(device);
    }

    public static IActionResult ToActionResultFromSessionResult(
        ControllerBase controller, Result<SimulationSession> result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<SimulationSession, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromActionResult(
        ControllerBase controller, Result<SimulationAction> result,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<SimulationAction, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode((DevicesError)result.Error!), result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetSessionResult(
        ControllerBase controller, SimulationSession? session, IEnumerable<SimulationAction> actions,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<SimulationSession, IEnumerable<SimulationAction>, IActionResult> successAction)
    {
        if (session is null)
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(DevicesError.SessionNotFound),
                DevicesError.SessionNotFound, errorLocalizer[nameof(DevicesError.SessionNotFound)]);
        return successAction(session, actions);
    }

    public static IActionResult ToActionResultFromGetActiveSessionResult(
        ControllerBase controller, SimulationSession? session, IEnumerable<SimulationAction> actions,
        IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory,
        Func<SimulationSession, IEnumerable<SimulationAction>, IActionResult> successAction)
    {
        if (session is null)
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCode(DevicesError.ActiveSessionNotFound),
                DevicesError.ActiveSessionNotFound, errorLocalizer[nameof(DevicesError.ActiveSessionNotFound)]);
        return successAction(session, actions);
    }
}
