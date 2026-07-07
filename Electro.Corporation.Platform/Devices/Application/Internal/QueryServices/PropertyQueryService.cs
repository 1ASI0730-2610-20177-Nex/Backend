using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Queries;
using Electro.Corporation.Platform.Devices.Domain.Repositories;

namespace Electro.Corporation.Platform.Devices.Application.Internal.QueryServices;

public class PropertyQueryService(IPropertyRepository propertyRepository) : IPropertyQueryService
{
    public async Task<Property?> Handle(GetPropertyByIdQuery query, CancellationToken cancellationToken)
    {
        return await propertyRepository.FindByIdAsync(query.PropertyId, cancellationToken);
    }

    public async Task<IEnumerable<Property>> Handle(GetPropertiesByUserIdQuery query,
        CancellationToken cancellationToken)
    {
        return await propertyRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }
}
