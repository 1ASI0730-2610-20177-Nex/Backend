namespace Electro.Corporation.Platform.Devices.Domain.Model.Commands;

public record UpdateHomeCommand(int Id, string Name, string Type, int UserId);
