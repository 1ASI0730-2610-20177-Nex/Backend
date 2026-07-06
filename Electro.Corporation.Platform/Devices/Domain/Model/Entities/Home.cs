using Electro.Corporation.Platform.Devices.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Entities;

public class Home
{
    public Home()
    {
        Name = string.Empty;
        Type = string.Empty;
    }

    public Home(CreateHomeCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        UserId = command.UserId;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int UserId { get; set; }

    public void Update(UpdateHomeCommand command)
    {
        Name = command.Name;
        Type = command.Type;
        UserId = command.UserId;
    }
}
