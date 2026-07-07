using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Shared.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Domain.Repositories;

public interface ISimulationSessionRepository : IBaseRepository<SimulationSession>
{
    Task<SimulationSession?> FindActiveByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
