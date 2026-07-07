using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Application.Internal.QueryServices;

public class SimulationSessionQueryService(
    ISimulationSessionRepository simulationSessionRepository,
    ISimulationActionRepository simulationActionRepository) : ISimulationSessionQueryService
{
    public async Task<(SimulationSession? Session, IEnumerable<SimulationAction> Actions)> Handle(
        GetSimulationSessionByIdQuery query, CancellationToken cancellationToken)
    {
        var session = await simulationSessionRepository.FindByIdAsync(query.SessionId, cancellationToken);
        if (session is null)
            return (null, []);

        var actions = await simulationActionRepository.FindBySessionIdAsync(query.SessionId, cancellationToken);
        return (session, actions);
    }

    public async Task<(SimulationSession? Session, IEnumerable<SimulationAction> Actions)> Handle(
        GetActiveSimulationSessionByUserIdQuery query, CancellationToken cancellationToken)
    {
        var session = await simulationSessionRepository.FindActiveByUserIdAsync(query.UserId, cancellationToken);
        if (session is null)
            return (null, []);

        var actions = await simulationActionRepository.FindBySessionIdAsync(session.Id, cancellationToken);
        return (session, actions);
    }
}
