using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Payment.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Payment.Application.QueryServices;

public interface ISubscriptionQueryService
{
    Task<Subscription?> Handle(GetActiveSubscriptionByUserIdQuery query, CancellationToken cancellationToken);
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<PaymentRecord>> Handle(GetPaymentsBySubscriptionIdQuery query, CancellationToken cancellationToken);
}


