using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Application.Internal.QueryServices;

public class HomeQueryService(IHomeRepository homeRepository) : IHomeQueryService
{
  public async Task<Home?> Handle(GetHomeByIdQuery query, CancellationToken cancellationToken)
  {
    return await homeRepository.FindByIdAsync(query.HomeId, cancellationToken);
  }

  public async Task<IEnumerable<Home>> Handle(GetAllHomesQuery query, CancellationToken cancellationToken)
  {
    return await homeRepository.ListAsync(cancellationToken);
  }
}
