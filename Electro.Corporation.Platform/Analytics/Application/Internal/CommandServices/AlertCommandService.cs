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

public class AlertCommandService(
    IAlertRepository alertRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IAlertCommandService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<Alert>> Handle(MarkAlertAsReadCommand command, CancellationToken cancellationToken)
  {
    var alert = await alertRepository.FindByIdAsync(command.AlertId, cancellationToken);
    if (alert is null)
      return Result<Alert>.Failure(AnalyticsError.AlertNotFound,
        _localizer[nameof(AnalyticsError.AlertNotFound)]);

    alert.MarkAsRead();
    try
    {
      alertRepository.Update(alert);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Alert>.Success(alert);
    }
    catch (OperationCanceledException)
    {
      return Result<Alert>.Failure(AnalyticsError.OperationCancelled,
        _localizer[nameof(AnalyticsError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Alert>.Failure(AnalyticsError.DatabaseError,
        _localizer[nameof(AnalyticsError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Alert>.Failure(AnalyticsError.InternalServerError,
        _localizer[nameof(AnalyticsError.InternalServerError)]);
    }
  }
}
