using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context)
  : BaseRepository<Alert>(context), IAlertRepository
{
  public async Task<IEnumerable<Alert>> FindByUserIdAsync(int userId, CancellationToken cancellationToken)
  {
    return await Context.Set<Alert>()
      .Where(a => a.UserId == userId)
      .OrderByDescending(a => a.CreatedAt)
      .ToListAsync(cancellationToken);
  }
}
