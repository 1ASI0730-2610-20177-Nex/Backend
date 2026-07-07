using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;

public static class PaymentResourceFromEntityAssembler
{
    public static PaymentResource ToResourceFromEntity(PaymentRecord entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity),
                "PaymentRecord entity cannot be null when converting to resource.");

        return new PaymentResource(
            entity.Id,
            entity.SubscriptionId,
            entity.Amount,
            entity.PaidAt,
            entity.Status,
            entity.PaymentMethod);
    }
}


