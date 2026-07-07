using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Analytics.Domain.Model.Entities;

public class Report
{
    public Report()
    {
        Title = string.Empty;
        Summary = string.Empty;
    }

    public Report(CreateReportCommand command)
    {
        PropertyId = command.PropertyId;
        Title = command.Title;
        Summary = command.Summary;
        GeneratedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public DateTime GeneratedAt { get; set; }
}
