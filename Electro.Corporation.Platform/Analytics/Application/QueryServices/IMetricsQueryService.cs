using Electro.Corporation.Platform.Analytics.Domain.Model;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Analytics.Application.QueryServices;

public interface IMetricsQueryService
{
    Task<Result<PropertyMetrics>> Handle(GetPropertyMetricsQuery query, CancellationToken cancellationToken);
}
