using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;

namespace Electro.Corporation.Platform.Analytics.Application.Internal.QueryServices;

public class ConsumptionQueryService(IConsumptionRepository consumptionRepository) : IConsumptionQueryService
{
  public async Task<Consumption?> Handle(GetConsumptionByIdQuery query, CancellationToken cancellationToken)
  {
    return await consumptionRepository.FindByIdAsync(query.ConsumptionId, cancellationToken);
  }

  public async Task<IEnumerable<Consumption>> Handle(GetAllConsumptionsQuery query,
    CancellationToken cancellationToken)
  {
    return await consumptionRepository.ListAsync(cancellationToken);
  }
}
