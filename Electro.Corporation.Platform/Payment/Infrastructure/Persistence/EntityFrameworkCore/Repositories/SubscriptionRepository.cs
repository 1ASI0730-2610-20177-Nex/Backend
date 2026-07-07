using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Payment.Domain.Model;
using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Payment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    public async Task<Subscription?> FindActiveByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Subscription>()
            .FirstOrDefaultAsync(
                s => s.UserId == userId
                     && s.Status == SubscriptionStatus.Active
                     && s.EndDate > DateTime.UtcNow,
                cancellationToken);
    }
}


