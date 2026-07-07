using Electro.Corporation.Platform.Devices.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Entities;

public class Property
{
    public Property()
    {
        Name = string.Empty;
        Type = string.Empty;
    }

    public Property(CreatePropertyCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        UserId = command.UserId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int UserId { get; set; }
}
