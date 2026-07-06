using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class HomeResourceFromEntityAssembler
{
  public static HomeResource ToResourceFromEntity(Home home)
  {
    return new HomeResource(home.Id, home.Name, home.Type, home.UserId);
  }
}

public static class CreateHomeCommandFromResourceAssembler
{
  public static CreateHomeCommand ToCommandFromResource(CreateHomeResource resource)
  {
    return new CreateHomeCommand(resource.Name, resource.Type, resource.UserId);
  }
}

public static class UpdateHomeCommandFromResourceAssembler
{
  public static UpdateHomeCommand ToCommandFromResource(int homeId, UpdateHomeResource resource)
  {
    return new UpdateHomeCommand(homeId, resource.Name, resource.Type, resource.UserId);
  }
}
