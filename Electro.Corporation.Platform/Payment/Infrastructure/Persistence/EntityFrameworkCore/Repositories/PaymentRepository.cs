using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Payment.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Payment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class PaymentRepository(AppDbContext context)
    : BaseRepository<PaymentRecord>(context), IPaymentRepository
{
    public async Task<IEnumerable<PaymentRecord>> FindBySubscriptionIdAsync(int subscriptionId,
        CancellationToken cancellationToken)
    {
        return await Context.Set<PaymentRecord>()
            .Where(p => p.SubscriptionId == subscriptionId)
            .OrderByDescending(p => p.PaidAt)
            .ToListAsync(cancellationToken);
    }
}


