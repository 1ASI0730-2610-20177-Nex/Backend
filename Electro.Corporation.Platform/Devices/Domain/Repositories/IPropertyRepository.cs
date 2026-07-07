using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Domain.Repositories;

public interface IPropertyRepository : IBaseRepository<Property>
{
    Task<IEnumerable<Property>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
