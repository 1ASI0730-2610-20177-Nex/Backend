using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Model;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Analytics.Application.Internal.QueryServices;

public class MetricsQueryService(
    AppDbContext context,
    IStringLocalizer<ErrorMessages> localizer) : IMetricsQueryService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<PropertyMetrics>> Handle(GetPropertyMetricsQuery query,
    CancellationToken cancellationToken)
  {
    var propertyExists = await context.Set<Property>()
      .AnyAsync(p => p.Id == query.PropertyId, cancellationToken);

    if (!propertyExists)
      return Result<PropertyMetrics>.Failure(AnalyticsError.PropertyNotFound,
        _localizer[nameof(AnalyticsError.PropertyNotFound)]);

    var deviceIds = await (
      from device in context.Set<Device>()
      join space in context.Set<Space>() on device.SpaceId equals space.Id
      where space.PropertyId == query.PropertyId
      select device.Id
    ).ToListAsync(cancellationToken);

    var deviceCount = deviceIds.Count;

    if (deviceCount == 0)
    {
      return Result<PropertyMetrics>.Success(new PropertyMetrics
      {
        PropertyId = query.PropertyId,
        TotalKwh = 0,
        DeviceCount = 0,
        AvgDailyKwh = 0
      });
    }

    var consumptions = await context.Set<Consumption>()
      .Where(c => deviceIds.Contains(c.DeviceId))
      .Select(c => new { c.Kwh, c.Date })
      .ToListAsync(cancellationToken);

    var totalKwh = consumptions.Sum(c => c.Kwh);
    var distinctDays = consumptions.Select(c => c.Date.Date).Distinct().Count();
    var avgDailyKwh = distinctDays > 0 ? totalKwh / distinctDays : 0;

    return Result<PropertyMetrics>.Success(new PropertyMetrics
    {
      PropertyId = query.PropertyId,
      TotalKwh = totalKwh,
      DeviceCount = deviceCount,
      AvgDailyKwh = avgDailyKwh
    });
  }
}
