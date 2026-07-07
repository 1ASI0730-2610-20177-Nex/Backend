namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record PropertyMetricsResource(
    int PropertyId,
    decimal TotalKwh,
    int DeviceCount,
    decimal AvgDailyKwh);
