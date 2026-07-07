namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record ReportResource(
    int Id,
    int PropertyId,
    string Title,
    string Summary,
    DateTime GeneratedAt);
