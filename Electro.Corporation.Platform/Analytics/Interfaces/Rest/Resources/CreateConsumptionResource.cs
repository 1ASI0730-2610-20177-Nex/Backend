namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record CreateConsumptionResource(int DeviceId, decimal Kwh, DateTime Date);
