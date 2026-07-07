using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "Subscription entity cannot be null when converting to resource.");

        return new SubscriptionResource(
            entity.Id,
            entity.UserId,
            entity.Plan,
            entity.Status,
            entity.StartDate,
            entity.EndDate,
            entity.MonthlyAmount);
    }
}


