using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Enums;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;

public partial class SimulationSession
{
    public SimulationSession()
    {
    }

    public SimulationSession(CreateSimulationSessionCommand command)
    {
        UserId = command.UserId;
        PropertyId = command.PropertyId;
        Status = SimulationSessionStatus.Active;
        StartedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public SimulationSessionStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public int PropertyId { get; private set; }

    public bool IsActive => Status == SimulationSessionStatus.Active;

    public void End()
    {
        Status = SimulationSessionStatus.Ended;
        EndedAt = DateTime.UtcNow;
    }
}
