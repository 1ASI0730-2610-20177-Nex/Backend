namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

public record CreateDeviceResource(string Name, string Type, int PowerWatts, string Status, int HomeId);
