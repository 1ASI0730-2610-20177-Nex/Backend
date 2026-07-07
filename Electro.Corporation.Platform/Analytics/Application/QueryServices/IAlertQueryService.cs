using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Analytics.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetAlertsByUserIdQuery query, CancellationToken cancellationToken);
}
