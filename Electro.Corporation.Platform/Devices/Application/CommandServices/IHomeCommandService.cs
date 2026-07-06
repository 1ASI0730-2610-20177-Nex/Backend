using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Devices.Application.CommandServices;

public interface IHomeCommandService
{
    Task<Result<Home>> Handle(CreateHomeCommand command, CancellationToken cancellationToken);
    Task<Result<Home>> Handle(UpdateHomeCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(DeleteHomeCommand command, CancellationToken cancellationToken);
}
