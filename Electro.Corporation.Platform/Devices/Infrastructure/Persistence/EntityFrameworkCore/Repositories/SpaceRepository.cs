using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SpaceRepository(AppDbContext context) : BaseRepository<Space>(context), ISpaceRepository
{
    public async Task<IEnumerable<Space>> FindByPropertyIdAsync(int propertyId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Space>()
            .Where(s => s.PropertyId == propertyId)
            .ToListAsync(cancellationToken);
    }
}
