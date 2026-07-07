using Electro.Corporation.Platform.Devices.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Entities;

public class Space
{
    public Space()
    {
        Name = string.Empty;
        Type = string.Empty;
    }

    public Space(CreateSpaceCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        PropertyId = command.PropertyId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int PropertyId { get; set; }
}
