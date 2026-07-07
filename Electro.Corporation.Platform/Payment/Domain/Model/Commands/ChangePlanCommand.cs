using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Domain.Model.Commands;

public record ChangePlanCommand(int SubscriptionId, SubscriptionPlan Plan);


