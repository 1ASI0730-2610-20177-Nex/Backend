using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Analytics.Application.QueryServices;

public interface IConsumptionQueryService
{
    Task<Consumption?> Handle(GetConsumptionByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Consumption>> Handle(GetAllConsumptionsQuery query, CancellationToken cancellationToken);
}
