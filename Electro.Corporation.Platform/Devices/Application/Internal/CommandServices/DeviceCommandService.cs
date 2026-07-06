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

public class DeviceCommandService(
    IDeviceRepository deviceRepository,
    IHomeRepository homeRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IDeviceCommandService
{
  private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

  public async Task<Result<Device>> Handle(CreateDeviceCommand command, CancellationToken cancellationToken)
  {
    var home = await homeRepository.FindByIdAsync(command.HomeId, cancellationToken);
    if (home is null)
      return Result<Device>.Failure(DevicesError.HomeNotFound,
        _localizer[nameof(DevicesError.HomeNotFound)]);

    var device = new Device(command);
    try
    {
      await deviceRepository.AddAsync(device, cancellationToken);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Device>.Success(device);
    }
    catch (OperationCanceledException)
    {
      return Result<Device>.Failure(DevicesError.OperationCancelled,
        _localizer[nameof(DevicesError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Device>.Failure(DevicesError.DatabaseError,
        _localizer[nameof(DevicesError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Device>.Failure(DevicesError.InternalServerError,
        _localizer[nameof(DevicesError.InternalServerError)]);
    }
  }

  public async Task<Result<Device>> Handle(UpdateDeviceCommand command, CancellationToken cancellationToken)
  {
    var device = await deviceRepository.FindByIdAsync(command.Id, cancellationToken);
    if (device is null)
      return Result<Device>.Failure(DevicesError.DeviceNotFound,
        _localizer[nameof(DevicesError.DeviceNotFound)]);

    var home = await homeRepository.FindByIdAsync(command.HomeId, cancellationToken);
    if (home is null)
      return Result<Device>.Failure(DevicesError.HomeNotFound,
        _localizer[nameof(DevicesError.HomeNotFound)]);

    device.Update(command);
    try
    {
      deviceRepository.Update(device);
      await unitOfWork.CompleteAsync(cancellationToken);
      return Result<Device>.Success(device);
    }
    catch (OperationCanceledException)
    {
      return Result<Device>.Failure(DevicesError.OperationCancelled,
        _localizer[nameof(DevicesError.OperationCancelled)]);
    }
    catch (DbUpdateException)
    {
      return Result<Device>.Failure(DevicesError.DatabaseError,
        _localizer[nameof(DevicesError.DatabaseError)]);
    }
    catch (Exception)
    {
      return Result<Device>.Failure(DevicesError.InternalServerError,
        _localizer[nameof(DevicesError.InternalServerError)]);
    }
  }

  public async Task<Result> Handle(DeleteDeviceCommand command, CancellationToken cancellationToken)
  {
    var device = await deviceRepository.FindByIdAsync(command.Id, cancellationToken);
    if (device is null)
      return Result.Failure(DevicesError.DeviceNotFound,
        _localizer[nameof(DevicesError.DeviceNotFound)]);

    try
    {
      deviceRepository.Remove(device);
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
