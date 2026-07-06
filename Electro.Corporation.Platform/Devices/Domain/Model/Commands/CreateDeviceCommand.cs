namespace Electro.Corporation.Platform.Devices.Domain.Model.Commands;

public record CreateDeviceCommand(string Name, string Type, int PowerWatts, string Status, int HomeId);
