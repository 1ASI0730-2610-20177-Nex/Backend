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

public class ConsumptionCommandService(
    IConsumptionRepository consumptionRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IConsumptionCommandService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<Consumption>> Handle(CreateConsumptionCommand command,
    CancellationToken cancellationToken)
  {
    var consumption = new Consumption(command);
    try
    {
      await consumptionRepository.AddAsync(consumption, cancellationToken);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Consumption>.Success(consumption);
    }
    catch (OperationCanceledException)
    {
      return Result<Consumption>.Failure(AnalyticsError.OperationCancelled,
        _localizer[nameof(AnalyticsError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Consumption>.Failure(AnalyticsError.DatabaseError,
        _localizer[nameof(AnalyticsError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Consumption>.Failure(AnalyticsError.InternalServerError,
        _localizer[nameof(AnalyticsError.InternalServerError)]);
    }
  }

  public async Task<Result<Consumption>> Handle(UpdateConsumptionCommand command,
    CancellationToken cancellationToken)
  {
    var consumption = await consumptionRepository.FindByIdAsync(command.Id, cancellationToken);
    if (consumption is null)
      return Result<Consumption>.Failure(AnalyticsError.ConsumptionNotFound,
        _localizer[nameof(AnalyticsError.ConsumptionNotFound)]);

    consumption.Update(command);
    try
    {
      consumptionRepository.Update(consumption);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Consumption>.Success(consumption);
    }
    catch (OperationCanceledException)
    {
      return Result<Consumption>.Failure(AnalyticsError.OperationCancelled,
        _localizer[nameof(AnalyticsError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Consumption>.Failure(AnalyticsError.DatabaseError,
        _localizer[nameof(AnalyticsError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Consumption>.Failure(AnalyticsError.InternalServerError,
        _localizer[nameof(AnalyticsError.InternalServerError)]);
    }
  }

  public async Task<Result> Handle(DeleteConsumptionCommand command, CancellationToken cancellationToken)
  {
    var consumption = await consumptionRepository.FindByIdAsync(command.Id, cancellationToken);
    if (consumption is null)
      return Result.Failure(AnalyticsError.ConsumptionNotFound,
        _localizer[nameof(AnalyticsError.ConsumptionNotFound)]);

    try
    {
      consumptionRepository.Remove(consumption);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result.Success();
    }
    catch (OperationCanceledException)
    {
      return Result.Failure(AnalyticsError.OperationCancelled,
        _localizer[nameof(AnalyticsError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result.Failure(AnalyticsError.DatabaseError,
        _localizer[nameof(AnalyticsError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result.Failure(AnalyticsError.InternalServerError,
        _localizer[nameof(AnalyticsError.InternalServerError)]);
    }
  }
}
