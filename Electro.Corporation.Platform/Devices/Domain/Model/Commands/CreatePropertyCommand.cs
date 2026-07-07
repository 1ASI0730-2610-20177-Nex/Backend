namespace Electro.Corporation.Platform.Devices.Domain.Model.Commands;

public record CreatePropertyCommand(string Name, string Type, int UserId);
