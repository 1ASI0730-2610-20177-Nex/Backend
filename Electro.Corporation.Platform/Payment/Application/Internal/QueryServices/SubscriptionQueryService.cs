using Electro.Corporation.Platform.Payment.Application.QueryServices;
using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Payment.Domain.Model.Queries;
using Electro.Corporation.Platform.Payment.Domain.Repositories;

namespace Electro.Corporation.Platform.Payment.Application.Internal.QueryServices;

public class SubscriptionQueryService(
    ISubscriptionRepository subscriptionRepository,
    IPaymentRepository paymentRepository) : ISubscriptionQueryService
{
    public async Task<Subscription?> Handle(GetActiveSubscriptionByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        return await subscriptionRepository.FindActiveByUserIdAsync(query.UserId, cancellationToken);
    }

    public async Task<Subscription?> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken)
    {
        return await subscriptionRepository.FindByIdAsync(query.SubscriptionId, cancellationToken);
    }

    public async Task<IEnumerable<PaymentRecord>> Handle(GetPaymentsBySubscriptionIdQuery query,
        CancellationToken cancellationToken)
    {
        return await paymentRepository.FindBySubscriptionIdAsync(query.SubscriptionId, cancellationToken);
    }
}


