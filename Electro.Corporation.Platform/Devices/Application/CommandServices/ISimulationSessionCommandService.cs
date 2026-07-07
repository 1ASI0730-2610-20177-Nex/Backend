using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Devices.Application.CommandServices;

public interface ISimulationSessionCommandService
{
    Task<Result<SimulationSession>> Handle(CreateSimulationSessionCommand command,
        CancellationToken cancellationToken);

    Task<Result<SimulationAction>> Handle(RegisterSimulationActionCommand command,
        CancellationToken cancellationToken);

    Task<Result<SimulationSession>> Handle(EndSimulationSessionCommand command,
        CancellationToken cancellationToken);
}
