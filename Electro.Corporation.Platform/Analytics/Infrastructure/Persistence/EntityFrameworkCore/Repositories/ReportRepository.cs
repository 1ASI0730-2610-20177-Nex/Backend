using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReportRepository(AppDbContext context)
  : BaseRepository<Report>(context), IReportRepository
{
  public async Task<IEnumerable<Report>> FindByPropertyIdAsync(int propertyId, CancellationToken cancellationToken)
  {
    return await Context.Set<Report>()
      .Where(r => r.PropertyId == propertyId)
      .OrderByDescending(r => r.GeneratedAt)
      .ToListAsync(cancellationToken);
  }
}
