using Electro.Corporation.Platform.Payment.Domain.Model.Commands;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;

public static class CreateSubscriptionCommandFromResourceAssembler
{
    public static CreateSubscriptionCommand ToCommandFromResource(CreateSubscriptionResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "CreateSubscriptionResource cannot be null when converting to command.");

        return new CreateSubscriptionCommand(resource.UserId, resource.Plan, resource.PaymentMethod);
    }
}


