using System.Net.Mime;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Electro.Corporation.Platform.Payment.Application.CommandServices;
using Electro.Corporation.Platform.Payment.Application.QueryServices;
using Electro.Corporation.Platform.Payment.Domain.Model.Commands;
using Electro.Corporation.Platform.Payment.Domain.Model.Queries;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Subscription management endpoints.")]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryService subscriptionQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;
    private readonly ProblemDetailsFactory _problemDetailsFactory = problemDetailsFactory;

    [HttpPost]
    [SwaggerOperation(Summary = "Create subscription", OperationId = "CreateSubscription")]
    [SwaggerResponse(StatusCodes.Status201Created, "Subscription created", typeof(SubscriptionResource))]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);
        return PaymentActionResultAssembler.ToActionResultFromSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            created => CreatedAtAction(nameof(GetActiveSubscription), new { userId = created.UserId },
                SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(created))
        );
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get active subscription for user", OperationId = "GetActiveSubscription")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active subscription found", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Subscription not found")]
    public async Task<IActionResult> GetActiveSubscription([FromQuery] int userId,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(
            new GetActiveSubscriptionByUserIdQuery(userId), cancellationToken);
        return PaymentActionResultAssembler.ToActionResultFromGetSubscriptionResult(
            this,
            subscription,
            _errorLocalizer,
            _problemDetailsFactory,
            found => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(found))
        );
    }

    [HttpPut("{subscriptionId:int}/renew")]
    [SwaggerOperation(Summary = "Renew subscription", OperationId = "RenewSubscription")]
    [SwaggerResponse(StatusCodes.Status200OK, "Subscription renewed", typeof(SubscriptionResource))]
    public async Task<IActionResult> RenewSubscription(int subscriptionId, CancellationToken cancellationToken)
    {
        var command = new RenewSubscriptionCommand(subscriptionId, "Card");
        var result = await subscriptionCommandService.Handle(command, cancellationToken);
        return PaymentActionResultAssembler.ToActionResultFromSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            renewed => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(renewed))
        );
    }

    [HttpPut("{subscriptionId:int}/plan")]
    [SwaggerOperation(Summary = "Change subscription plan", OperationId = "ChangeSubscriptionPlan")]
    [SwaggerResponse(StatusCodes.Status200OK, "Plan changed", typeof(SubscriptionResource))]
    public async Task<IActionResult> ChangeSubscriptionPlan(int subscriptionId,
        [FromBody] ChangePlanResource resource, CancellationToken cancellationToken)
    {
        var command = ChangePlanCommandFromResourceAssembler.ToCommandFromResource(subscriptionId, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);
        return PaymentActionResultAssembler.ToActionResultFromSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            updated => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(updated))
        );
    }

    [HttpPut("{subscriptionId:int}/cancel")]
    [SwaggerOperation(Summary = "Cancel subscription", OperationId = "CancelSubscription")]
    [SwaggerResponse(StatusCodes.Status200OK, "Subscription cancelled", typeof(SubscriptionResource))]
    public async Task<IActionResult> CancelSubscription(int subscriptionId, CancellationToken cancellationToken)
    {
        var result = await subscriptionCommandService.Handle(
            new CancelSubscriptionCommand(subscriptionId), cancellationToken);
        return PaymentActionResultAssembler.ToActionResultFromSubscriptionResult(
            this,
            result,
            _errorLocalizer,
            _problemDetailsFactory,
            cancelled => Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(cancelled))
        );
    }

    [HttpGet("{subscriptionId:int}/payments")]
    [SwaggerOperation(Summary = "Get payment history", OperationId = "GetSubscriptionPayments")]
    [SwaggerResponse(StatusCodes.Status200OK, "Payment history", typeof(IEnumerable<PaymentResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Subscription not found")]
    public async Task<IActionResult> GetSubscriptionPayments(int subscriptionId,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionQueryService.Handle(
            new GetSubscriptionByIdQuery(subscriptionId), cancellationToken);
        if (subscription is null)
            return PaymentActionResultAssembler.ToActionResultFromGetSubscriptionResult(
                this,
                null,
                _errorLocalizer,
                _problemDetailsFactory,
                _ => Ok());

        var payments = await subscriptionQueryService.Handle(
            new GetPaymentsBySubscriptionIdQuery(subscriptionId), cancellationToken);
        return Ok(payments.Select(PaymentResourceFromEntityAssembler.ToResourceFromEntity));
    }
}


