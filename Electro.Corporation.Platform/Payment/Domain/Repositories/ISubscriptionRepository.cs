using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Payment.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<Subscription?> FindActiveByUserIdAsync(int userId, CancellationToken cancellationToken);
}


