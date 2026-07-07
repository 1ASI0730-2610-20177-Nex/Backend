using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Analytics.Application.CommandServices;

public interface IReportCommandService
{
    Task<Result<Report>> Handle(CreateReportCommand command, CancellationToken cancellationToken);
}
