using Electro.Corporation.Platform.Iam.Interfaces.Acl;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Electro.Corporation.Platform.Payment.Application.CommandServices;
using Electro.Corporation.Platform.Payment.Domain.Model;
using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Model.Commands;
using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Payment.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Payment.Application.Internal.CommandServices;

public class SubscriptionCommandService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRepository paymentRepository,
    IIamContextFacade iamContextFacade,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : ISubscriptionCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Subscription>> Handle(CreateSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var username = await iamContextFacade.FetchUsernameByUserId(command.UserId, cancellationToken);
        if (string.IsNullOrEmpty(username))
            return Result<Subscription>.Failure(PaymentError.UserNotFound,
                _localizer[nameof(PaymentError.UserNotFound)]);

        if (!Enum.IsDefined(typeof(SubscriptionPlan), command.Plan))
            return Result<Subscription>.Failure(PaymentError.InvalidPlan,
                _localizer[nameof(PaymentError.InvalidPlan)]);

        var existingSubscription =
            await subscriptionRepository.FindActiveByUserIdAsync(command.UserId, cancellationToken);
        if (existingSubscription is not null)
            return Result<Subscription>.Failure(PaymentError.ActiveSubscriptionAlreadyExists,
                _localizer[nameof(PaymentError.ActiveSubscriptionAlreadyExists)]);

        var subscription = new Subscription(command);
        return await SaveSubscriptionWithPaymentAsync(subscription, command.PaymentMethod, cancellationToken);
    }

    public async Task<Result<Subscription>> Handle(RenewSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(PaymentError.SubscriptionNotFound,
                _localizer[nameof(PaymentError.SubscriptionNotFound)]);

        if (subscription.Status == SubscriptionStatus.Cancelled)
            return Result<Subscription>.Failure(PaymentError.SubscriptionAlreadyCancelled,
                _localizer[nameof(PaymentError.SubscriptionAlreadyCancelled)]);

        subscription.Renew();
        return await SaveSubscriptionWithPaymentAsync(subscription, command.PaymentMethod, cancellationToken,
            update: true);
    }

    public async Task<Result<Subscription>> Handle(ChangePlanCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(PaymentError.SubscriptionNotFound,
                _localizer[nameof(PaymentError.SubscriptionNotFound)]);

        if (subscription.Status != SubscriptionStatus.Active)
            return Result<Subscription>.Failure(PaymentError.SubscriptionAlreadyCancelled,
                _localizer[nameof(PaymentError.SubscriptionAlreadyCancelled)]);

        if (!Enum.IsDefined(typeof(SubscriptionPlan), command.Plan))
            return Result<Subscription>.Failure(PaymentError.InvalidPlan,
                _localizer[nameof(PaymentError.InvalidPlan)]);

        subscription.ChangePlan(command);
        return await SaveSubscriptionAsync(subscription, cancellationToken, update: true);
    }

    public async Task<Result<Subscription>> Handle(CancelSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId, cancellationToken);
        if (subscription is null)
            return Result<Subscription>.Failure(PaymentError.SubscriptionNotFound,
                _localizer[nameof(PaymentError.SubscriptionNotFound)]);

        if (subscription.Status == SubscriptionStatus.Cancelled)
            return Result<Subscription>.Failure(PaymentError.SubscriptionAlreadyCancelled,
                _localizer[nameof(PaymentError.SubscriptionAlreadyCancelled)]);

        subscription.Cancel();
        return await SaveSubscriptionAsync(subscription, cancellationToken, update: true);
    }

    private async Task<Result<Subscription>> SaveSubscriptionWithPaymentAsync(Subscription subscription,
        string paymentMethod, CancellationToken cancellationToken, bool update = false)
    {
        try
        {
            if (update)
                subscriptionRepository.Update(subscription);
            else
                await subscriptionRepository.AddAsync(subscription, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            var payment = new PaymentRecord(subscription.Id, subscription.MonthlyAmount, PaymentStatus.Completed,
                paymentMethod);
            await paymentRepository.AddAsync(payment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(PaymentError.OperationCancelled,
                _localizer[nameof(PaymentError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(PaymentError.DatabaseError,
                _localizer[nameof(PaymentError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(PaymentError.InternalServerError,
                _localizer[nameof(PaymentError.InternalServerError)]);
        }
    }

    private async Task<Result<Subscription>> SaveSubscriptionAsync(Subscription subscription,
        CancellationToken cancellationToken, bool update = false)
    {
        try
        {
            if (update)
                subscriptionRepository.Update(subscription);
            else
                await subscriptionRepository.AddAsync(subscription, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Subscription>.Success(subscription);
        }
        catch (OperationCanceledException)
        {
            return Result<Subscription>.Failure(PaymentError.OperationCancelled,
                _localizer[nameof(PaymentError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Subscription>.Failure(PaymentError.DatabaseError,
                _localizer[nameof(PaymentError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Subscription>.Failure(PaymentError.InternalServerError,
                _localizer[nameof(PaymentError.InternalServerError)]);
        }
    }
}


