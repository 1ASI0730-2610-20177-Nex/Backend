using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Domain.Model;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Devices.Application.Internal.CommandServices;

public class HomeCommandService(
    IHomeRepository homeRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IHomeCommandService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<Home>> Handle(CreateHomeCommand command, CancellationToken cancellationToken)
  {
    var home = new Home(command);
    try
    {
      await homeRepository.AddAsync(home, cancellationToken);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Home>.Success(home);
    }
    catch (OperationCanceledException)
    {
      return Result<Home>.Failure(DevicesError.OperationCancelled,
        _localizer[nameof(DevicesError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Home>.Failure(DevicesError.DatabaseError,
        _localizer[nameof(DevicesError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Home>.Failure(DevicesError.InternalServerError,
        _localizer[nameof(DevicesError.InternalServerError)]);
    }
  }

  public async Task<Result<Home>> Handle(UpdateHomeCommand command, CancellationToken cancellationToken)
  {
    var home = await homeRepository.FindByIdAsync(command.Id, cancellationToken);
    if (home is null)
      return Result<Home>.Failure(DevicesError.HomeNotFound,
        _localizer[nameof(DevicesError.HomeNotFound)]);

    home.Update(command);
    try
    {
      homeRepository.Update(home);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Home>.Success(home);
    }
    catch (OperationCanceledException)
    {
      return Result<Home>.Failure(DevicesError.OperationCancelled,
        _localizer[nameof(DevicesError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Home>.Failure(DevicesError.DatabaseError,
        _localizer[nameof(DevicesError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Home>.Failure(DevicesError.InternalServerError,
        _localizer[nameof(DevicesError.InternalServerError)]);
    }
  }

  public async Task<Result> Handle(DeleteHomeCommand command, CancellationToken cancellationToken)
  {
    var home = await homeRepository.FindByIdAsync(command.Id, cancellationToken);
    if (home is null)
      return Result.Failure(DevicesError.HomeNotFound,
        _localizer[nameof(DevicesError.HomeNotFound)]);

    try
    {
      homeRepository.Remove(home);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result.Success();
    }
    catch (OperationCanceledException)
    {
      return Result.Failure(DevicesError.OperationCancelled,
        _localizer[nameof(DevicesError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result.Failure(DevicesError.DatabaseError,
        _localizer[nameof(DevicesError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result.Failure(DevicesError.InternalServerError,
        _localizer[nameof(DevicesError.InternalServerError)]);
    }
  }
}
