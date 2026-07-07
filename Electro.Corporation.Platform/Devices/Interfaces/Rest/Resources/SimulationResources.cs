namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

public record SimulationSessionResource(
    int Id,
    int UserId,
    string Status,
    DateTime StartedAt,
    DateTime? EndedAt,
    int PropertyId,
    IEnumerable<SimulationActionResource> Actions);

public record SimulationActionResource(
    int Id,
    int SessionId,
    string ActionType,
    string Description,
    DateTime CreatedAt);

public record CreateSimulationSessionResource(int UserId, int PropertyId);
public record CreateSimulationActionResource(string ActionType, string Description);
