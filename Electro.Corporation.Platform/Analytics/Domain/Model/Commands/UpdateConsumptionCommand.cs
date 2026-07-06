namespace Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

public record UpdateConsumptionCommand(int Id, int DeviceId, decimal Kwh, DateTime Date);
