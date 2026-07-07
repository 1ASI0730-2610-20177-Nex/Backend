namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

public record PropertyResource(int Id, string Name, string Type, int UserId);
public record CreatePropertyResource(string Name, string Type, int UserId);
public record SpaceResource(int Id, string Name, string Type, int PropertyId);
public record CreateSpaceResource(string Name, string Type);
