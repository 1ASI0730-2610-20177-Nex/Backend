namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record ConsumptionResource(int Id, int DeviceId, decimal Kwh, DateTime Date);
