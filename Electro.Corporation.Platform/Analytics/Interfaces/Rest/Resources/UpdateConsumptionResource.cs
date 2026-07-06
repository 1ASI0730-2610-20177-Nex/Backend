namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

public record UpdateConsumptionResource(int DeviceId, decimal Kwh, DateTime Date);
