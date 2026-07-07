using Electro.Corporation.Platform.Analytics.Domain.Model;
using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Transform;

public static class AlertResourceFromEntityAssembler
{
  public static AlertResource ToResourceFromEntity(Alert alert)
  {
    return new AlertResource(
      alert.Id,
      alert.UserId,
      alert.PropertyId,
      alert.Title,
      alert.Message,
      alert.IsRead,
      alert.CreatedAt);
  }
}

public static class ReportResourceFromEntityAssembler
{
  public static ReportResource ToResourceFromEntity(Report report)
  {
    return new ReportResource(
      report.Id,
      report.PropertyId,
      report.Title,
      report.Summary,
      report.GeneratedAt);
  }
}

public static class CreateReportCommandFromResourceAssembler
{
  public static CreateReportCommand ToCommandFromResource(CreateReportResource resource)
  {
    return new CreateReportCommand(resource.PropertyId, resource.Title, resource.Summary);
  }
}

public static class PropertyMetricsResourceFromEntityAssembler
{
  public static PropertyMetricsResource ToResourceFromEntity(PropertyMetrics metrics)
  {
    return new PropertyMetricsResource(
      metrics.PropertyId,
      metrics.TotalKwh,
      metrics.DeviceCount,
      metrics.AvgDailyKwh);
  }
}
