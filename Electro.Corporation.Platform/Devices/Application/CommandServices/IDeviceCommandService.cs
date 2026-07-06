using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Devices.Application.CommandServices;

public interface IDeviceCommandService
{
    Task<Result<Device>> Handle(CreateDeviceCommand command, CancellationToken cancellationToken);
    Task<Result<Device>> Handle(UpdateDeviceCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(DeleteDeviceCommand command, CancellationToken cancellationToken);
}
