namespace Electro.Corporation.Platform.Devices.Domain.Model.Commands;

public record UpdateDeviceCommand(int Id, string Name, string Type, int PowerWatts, string Status, int SpaceId);
