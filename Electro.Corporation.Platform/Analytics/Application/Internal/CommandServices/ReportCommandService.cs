using Electro.Corporation.Platform.Analytics.Application.CommandServices;
using Electro.Corporation.Platform.Analytics.Domain.Model;
using Electro.Corporation.Platform.Analytics.Domain.Model.Commands;
using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Analytics.Application.Internal.CommandServices;

public class ReportCommandService(
    IReportRepository reportRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IReportCommandService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<Report>> Handle(CreateReportCommand command, CancellationToken cancellationToken)
  {
    var report = new Report(command);
    try
    {
      await reportRepository.AddAsync(report, cancellationToken);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Report>.Success(report);
    }
    catch (OperationCanceledException)
    {
      return Result<Report>.Failure(AnalyticsError.OperationCancelled,
        _localizer[nameof(AnalyticsError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Report>.Failure(AnalyticsError.DatabaseError,
        _localizer[nameof(AnalyticsError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Report>.Failure(AnalyticsError.InternalServerError,
        _localizer[nameof(AnalyticsError.InternalServerError)]);
    }
  }
}
