using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Devices.Application.QueryServices;

public interface IPropertyQueryService
{
    Task<Property?> Handle(GetPropertyByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Property>> Handle(GetPropertiesByUserIdQuery query, CancellationToken cancellationToken);
}
