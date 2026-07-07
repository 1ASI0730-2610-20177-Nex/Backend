using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;

namespace Electro.Corporation.Platform.Analytics.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
  public async Task<IEnumerable<Alert>> Handle(GetAlertsByUserIdQuery query, CancellationToken cancellationToken)
  {
    return await alertRepository.FindByUserIdAsync(query.UserId, cancellationToken);
  }
}
