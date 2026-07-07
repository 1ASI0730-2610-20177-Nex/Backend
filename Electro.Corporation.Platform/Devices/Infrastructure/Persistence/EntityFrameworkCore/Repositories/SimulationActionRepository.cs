using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SimulationActionRepository(AppDbContext context)
    : BaseRepository<SimulationAction>(context), ISimulationActionRepository
{
    public async Task<IEnumerable<SimulationAction>> FindBySessionIdAsync(int sessionId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SimulationAction>()
            .Where(a => a.SessionId == sessionId)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
