using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;


public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}