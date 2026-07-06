using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Devices.Application.QueryServices;

public interface IDeviceQueryService
{
    Task<Device?> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Device>> Handle(GetAllDevicesQuery query, CancellationToken cancellationToken);
}
