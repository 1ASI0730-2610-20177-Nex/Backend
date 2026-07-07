using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

public record SubscriptionResource(
    int Id,
    int UserId,
    SubscriptionPlan Plan,
    SubscriptionStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    decimal MonthlyAmount);


