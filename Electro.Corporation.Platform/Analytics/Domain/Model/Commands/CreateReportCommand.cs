namespace Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

public record CreateReportCommand(int PropertyId, string Title, string Summary);
