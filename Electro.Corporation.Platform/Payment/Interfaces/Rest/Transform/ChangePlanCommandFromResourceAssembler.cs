using Electro.Corporation.Platform.Payment.Domain.Model.Commands;
using Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Transform;

public static class ChangePlanCommandFromResourceAssembler
{
    public static ChangePlanCommand ToCommandFromResource(int subscriptionId, ChangePlanResource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource),
                "ChangePlanResource cannot be null when converting to command.");

        return new ChangePlanCommand(subscriptionId, resource.Plan);
    }
}


