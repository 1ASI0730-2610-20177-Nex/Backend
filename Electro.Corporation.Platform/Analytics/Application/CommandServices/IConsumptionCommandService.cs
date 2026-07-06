using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Analytics.Application.CommandServices;

public interface IConsumptionCommandService
{
    Task<Result<Consumption>> Handle(CreateConsumptionCommand command, CancellationToken cancellationToken);
    Task<Result<Consumption>> Handle(UpdateConsumptionCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(DeleteConsumptionCommand command, CancellationToken cancellationToken);
}
