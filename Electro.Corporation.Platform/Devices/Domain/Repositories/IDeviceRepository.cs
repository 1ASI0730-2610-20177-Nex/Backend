using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Domain.Repositories;

public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<IEnumerable<Device>> FindBySpaceIdAsync(int spaceId, CancellationToken cancellationToken = default);
}
