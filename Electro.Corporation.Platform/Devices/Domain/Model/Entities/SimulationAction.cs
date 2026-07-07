using Electro.Corporation.Platform.Devices.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Entities;

public class SimulationAction
{
    public SimulationAction()
    {
        ActionType = string.Empty;
        Description = string.Empty;
    }

    public SimulationAction(RegisterSimulationActionCommand command)
    {
        SessionId = command.SessionId;
        ActionType = command.ActionType;
        Description = command.Description;
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int SessionId { get; set; }
    public string ActionType { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
