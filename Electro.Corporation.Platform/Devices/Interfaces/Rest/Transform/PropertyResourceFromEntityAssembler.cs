using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class PropertyResourceFromEntityAssembler
{
    public static PropertyResource ToResourceFromEntity(Property property)
    {
        return new PropertyResource(property.Id, property.Name, property.Type, property.UserId);
    }
}

public static class CreatePropertyCommandFromResourceAssembler
{
    public static CreatePropertyCommand ToCommandFromResource(CreatePropertyResource resource)
    {
        return new CreatePropertyCommand(resource.Name, resource.Type, resource.UserId);
    }
}

public static class SpaceResourceFromEntityAssembler
{
    public static SpaceResource ToResourceFromEntity(Space space)
    {
        return new SpaceResource(space.Id, space.Name, space.Type, space.PropertyId);
    }
}

public static class CreateSpaceCommandFromResourceAssembler
{
    public static CreateSpaceCommand ToCommandFromResource(int propertyId, CreateSpaceResource resource)
    {
        return new CreateSpaceCommand(resource.Name, resource.Type, propertyId);
    }
}
