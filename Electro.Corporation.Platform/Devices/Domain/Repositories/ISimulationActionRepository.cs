using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Domain.Repositories;

public interface ISimulationActionRepository : IBaseRepository<SimulationAction>
{
    Task<IEnumerable<SimulationAction>> FindBySessionIdAsync(int sessionId,
        CancellationToken cancellationToken = default);
}
