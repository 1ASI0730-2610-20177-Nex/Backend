using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class DeviceResourceFromEntityAssembler
{
  public static DeviceResource ToResourceFromEntity(Device device)
  {
    return new DeviceResource(device.Id, device.Name, device.Type, device.PowerWatts, device.Status,
      device.HomeId);
  }
}

public static class CreateDeviceCommandFromResourceAssembler
{
  public static CreateDeviceCommand ToCommandFromResource(CreateDeviceResource resource)
  {
    return new CreateDeviceCommand(resource.Name, resource.Type, resource.PowerWatts, resource.Status,
      resource.HomeId);
  }
}

public static class UpdateDeviceCommandFromResourceAssembler
{
  public static UpdateDeviceCommand ToCommandFromResource(int deviceId, UpdateDeviceResource resource)
  {
    return new UpdateDeviceCommand(deviceId, resource.Name, resource.Type, resource.PowerWatts, resource.Status,
      resource.HomeId);
  }
}
