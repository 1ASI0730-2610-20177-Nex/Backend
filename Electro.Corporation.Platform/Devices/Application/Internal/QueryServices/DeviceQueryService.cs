using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Application.Internal.QueryServices;

public class DeviceQueryService(IDeviceRepository deviceRepository) : IDeviceQueryService
{
  public async Task<Device?> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken)
  {
    return await deviceRepository.FindByIdAsync(query.DeviceId, cancellationToken);
  }

  public async Task<IEnumerable<Device>> Handle(GetAllDevicesQuery query, CancellationToken cancellationToken)
  {
    return await deviceRepository.ListAsync(cancellationToken);
  }
}
