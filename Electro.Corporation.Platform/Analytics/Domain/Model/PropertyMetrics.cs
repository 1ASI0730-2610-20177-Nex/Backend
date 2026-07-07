namespace Electro.Corporation.Platform.Analytics.Domain.Model;

public class PropertyMetrics
{
    public int PropertyId { get; set; }
    public decimal TotalKwh { get; set; }
    public int DeviceCount { get; set; }
    public decimal AvgDailyKwh { get; set; }
}
