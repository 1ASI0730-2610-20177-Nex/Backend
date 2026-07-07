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

public class SpaceCommandService(
    ISpaceRepository spaceRepository,
    IPropertyRepository propertyRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : ISpaceCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Space>> Handle(CreateSpaceCommand command, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.FindByIdAsync(command.PropertyId, cancellationToken);
        if (property is null)
            return Result<Space>.Failure(DevicesError.PropertyNotFound,
                _localizer[nameof(DevicesError.PropertyNotFound)]);

        var space = new Space(command);
        try
        {
            await spaceRepository.AddAsync(space, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Space>.Success(space);
        }
        catch (OperationCanceledException)
        {
            return Result<Space>.Failure(DevicesError.OperationCancelled,
                _localizer[nameof(DevicesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Space>.Failure(DevicesError.DatabaseError,
                _localizer[nameof(DevicesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Space>.Failure(DevicesError.InternalServerError,
                _localizer[nameof(DevicesError.InternalServerError)]);
        }
    }
}
