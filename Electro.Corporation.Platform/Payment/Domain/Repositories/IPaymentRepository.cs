using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Payment.Domain.Repositories;

public interface IPaymentRepository : IBaseRepository<PaymentRecord>
{
    Task<IEnumerable<PaymentRecord>> FindBySubscriptionIdAsync(int subscriptionId, CancellationToken cancellationToken);
}


