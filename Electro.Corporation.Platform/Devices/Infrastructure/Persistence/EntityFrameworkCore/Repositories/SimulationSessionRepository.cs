using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Enums;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SimulationSessionRepository(AppDbContext context)
    : BaseRepository<SimulationSession>(context), ISimulationSessionRepository
{
    public async Task<SimulationSession?> FindActiveByUserIdAsync(int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<SimulationSession>()
            .FirstOrDefaultAsync(
                s => s.UserId == userId && s.Status == SimulationSessionStatus.Active,
                cancellationToken);
    }
}
