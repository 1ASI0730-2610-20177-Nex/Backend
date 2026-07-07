namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record AlertResource(
    int Id,
    int UserId,
    int PropertyId,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt);
