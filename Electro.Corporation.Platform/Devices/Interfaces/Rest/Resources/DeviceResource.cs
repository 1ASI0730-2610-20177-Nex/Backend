namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

public record DeviceResource(int Id, string Name, string Type, int PowerWatts, string Status, int HomeId);
