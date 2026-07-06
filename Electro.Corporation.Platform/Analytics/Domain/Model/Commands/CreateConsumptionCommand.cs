namespace Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

public record CreateConsumptionCommand(int DeviceId, decimal Kwh, DateTime Date);
