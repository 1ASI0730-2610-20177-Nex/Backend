using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Analytics.Interfaces.Rest.Transform;

public static class ConsumptionResourceFromEntityAssembler
{
  public static ConsumptionResource ToResourceFromEntity(Consumption consumption)
  {
    return new ConsumptionResource(consumption.Id, consumption.DeviceId, consumption.Kwh, consumption.Date);
  }
}

public static class CreateConsumptionCommandFromResourceAssembler
{
  public static CreateConsumptionCommand ToCommandFromResource(CreateConsumptionResource resource)
  {
    return new CreateConsumptionCommand(resource.DeviceId, resource.Kwh, resource.Date);
  }
}

public static class UpdateConsumptionCommandFromResourceAssembler
{
  public static UpdateConsumptionCommand ToCommandFromResource(int consumptionId,
    UpdateConsumptionResource resource)
  {
    return new UpdateConsumptionCommand(consumptionId, resource.DeviceId, resource.Kwh, resource.Date);
  }
}
