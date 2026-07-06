using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Devices.Application.QueryServices;

public interface IHomeQueryService
{
    Task<Home?> Handle(GetHomeByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Home>> Handle(GetAllHomesQuery query, CancellationToken cancellationToken);
}
