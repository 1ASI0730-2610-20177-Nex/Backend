namespace Electro.Corporation.Platform.Devices.Domain.Model.Commands;

public record RegisterSimulationActionCommand(int SessionId, string ActionType, string Description);
