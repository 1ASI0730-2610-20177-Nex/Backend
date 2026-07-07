using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Analytics.Application.QueryServices;

public interface IReportQueryService
{
    Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Report>> Handle(GetReportsByPropertyIdQuery query, CancellationToken cancellationToken);
}
