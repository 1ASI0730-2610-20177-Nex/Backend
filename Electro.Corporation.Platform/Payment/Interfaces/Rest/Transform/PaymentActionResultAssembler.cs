using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Electro.Corporation.Platform.Payment.Domain.Model;
using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;

public static class PaymentActionResultAssembler
{
    private static int ToStatusCodeFromPaymentError(PaymentError error)
    {
        return error switch
        {
            PaymentError.SubscriptionNotFound => StatusCodes.Status404NotFound,
            PaymentError.UserNotFound => StatusCodes.Status404NotFound,
            PaymentError.ActiveSubscriptionAlreadyExists => StatusCodes.Status409Conflict,
            PaymentError.SubscriptionAlreadyCancelled => StatusCodes.Status409Conflict,
            PaymentError.InvalidPlan => StatusCodes.Status400BadRequest,
            PaymentError.OperationCancelled => StatusCodes.Status409Conflict,
            PaymentError.DatabaseError => StatusCodes.Status500InternalServerError,
            PaymentError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromSubscriptionResult(
        ControllerBase controller,
        Result<Subscription> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Subscription, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromPaymentError((PaymentError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetSubscriptionResult(
        ControllerBase controller,
        Subscription? subscription,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Subscription, IActionResult> successAction)
    {
        if (subscription is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromPaymentError(PaymentError.SubscriptionNotFound),
                PaymentError.SubscriptionNotFound,
                errorLocalizer[nameof(PaymentError.SubscriptionNotFound)]
            );

        return successAction(subscription);
    }
}


