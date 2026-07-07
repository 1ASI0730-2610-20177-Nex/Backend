using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Model.Commands;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Payment.Application.CommandServices;

public interface ISubscriptionCommandService
{
    Task<Result<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken);
    Task<Result<Subscription>> Handle(RenewSubscriptionCommand command, CancellationToken cancellationToken);
    Task<Result<Subscription>> Handle(ChangePlanCommand command, CancellationToken cancellationToken);
    Task<Result<Subscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken);
}


