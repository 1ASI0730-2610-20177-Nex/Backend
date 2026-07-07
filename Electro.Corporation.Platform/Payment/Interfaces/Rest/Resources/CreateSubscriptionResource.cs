using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

public record CreateSubscriptionResource(int UserId, SubscriptionPlan Plan, string PaymentMethod);


