using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Devices.Application.CommandServices;

public interface ISpaceCommandService
{
    Task<Result<Space>> Handle(CreateSpaceCommand command, CancellationToken cancellationToken);
}
