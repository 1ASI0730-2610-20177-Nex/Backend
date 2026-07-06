using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Analytics.Domain.Model.Entities;

public class Consumption
{
    public Consumption()
    {
    }

    public Consumption(CreateConsumptionCommand command)
    {
        DeviceId = command.DeviceId;
        Kwh = command.Kwh;
        Date = command.Date;
    }

    public int Id { get; set; }
    public int DeviceId { get; set; }
    public decimal Kwh { get; set; }
    public DateTime Date { get; set; }

    public void Update(UpdateConsumptionCommand command)
    {
        DeviceId = command.DeviceId;
        Kwh = command.Kwh;
        Date = command.Date;
    }
}
