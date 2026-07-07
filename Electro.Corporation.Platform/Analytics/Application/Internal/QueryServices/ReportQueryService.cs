using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Model.Queries;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;

namespace Electro.Corporation.Platform.Analytics.Application.Internal.QueryServices;

public class ReportQueryService(IReportRepository reportRepository) : IReportQueryService
{
  public async Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken)
  {
    return await reportRepository.FindByIdAsync(query.ReportId, cancellationToken);
  }

  public async Task<IEnumerable<Report>> Handle(GetReportsByPropertyIdQuery query,
    CancellationToken cancellationToken)
  {
    return await reportRepository.FindByPropertyIdAsync(query.PropertyId, cancellationToken);
  }
}
