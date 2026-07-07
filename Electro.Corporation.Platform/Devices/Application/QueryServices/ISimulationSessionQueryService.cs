using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Devices.Application.QueryServices;

public interface ISimulationSessionQueryService
{
    Task<(SimulationSession? Session, IEnumerable<SimulationAction> Actions)> Handle(
        GetSimulationSessionByIdQuery query, CancellationToken cancellationToken);

    Task<(SimulationSession? Session, IEnumerable<SimulationAction> Actions)> Handle(
        GetActiveSimulationSessionByUserIdQuery query, CancellationToken cancellationToken);
}
