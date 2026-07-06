using Electro.Corporation.Platform.Devices.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Entities;

public class Device
{
    public Device()
    {
        Name = string.Empty;
        Type = string.Empty;
        Status = string.Empty;
    }

    public Device(CreateDeviceCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        PowerWatts = command.PowerWatts;
        Status = command.Status;
        HomeId = command.HomeId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int PowerWatts { get; set; }
    public string Status { get; set; }
    public int HomeId { get; set; }

    public void Update(UpdateDeviceCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        PowerWatts = command.PowerWatts;
        Status = command.Status;
        HomeId = command.HomeId;
    }
}
