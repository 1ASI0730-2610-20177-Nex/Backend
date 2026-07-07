using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Domain.Model.Commands;

public record CreateSubscriptionCommand(int UserId, SubscriptionPlan Plan, string PaymentMethod);


