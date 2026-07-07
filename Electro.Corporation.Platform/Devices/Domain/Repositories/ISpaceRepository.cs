using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Domain.Repositories;

public interface ISpaceRepository : IBaseRepository<Space>
{
    Task<IEnumerable<Space>> FindByPropertyIdAsync(int propertyId, CancellationToken cancellationToken = default);
}
