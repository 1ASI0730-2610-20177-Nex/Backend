using Electro.Corporation.Platform.Payment.Domain.Model;
using Electro.Corporation.Platform.Payment.Domain.Model.Commands;

namespace Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;

public class Subscription
{
    public Subscription()
    {
    }

    public Subscription(CreateSubscriptionCommand command)
    {
        UserId = command.UserId;
        Plan = command.Plan;
        Status = SubscriptionStatus.Active;
        StartDate = DateTime.UtcNow;
        EndDate = StartDate.AddMonths(1);
        MonthlyAmount = GetMonthlyAmount(command.Plan);
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public SubscriptionPlan Plan { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal MonthlyAmount { get; private set; }

    public void Renew()
    {
        var renewalStart = EndDate > DateTime.UtcNow ? EndDate : DateTime.UtcNow;
        EndDate = renewalStart.AddMonths(1);
        Status = SubscriptionStatus.Active;
    }

    public void ChangePlan(ChangePlanCommand command)
    {
        Plan = command.Plan;
        MonthlyAmount = GetMonthlyAmount(command.Plan);
    }

    public void Cancel()
    {
        Status = SubscriptionStatus.Cancelled;
    }

    public static decimal GetMonthlyAmount(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Basic => 9.99m,
        SubscriptionPlan.Standard => 19.99m,
        SubscriptionPlan.Premium => 29.99m,
        _ => throw new ArgumentOutOfRangeException(nameof(plan))
    };
}


