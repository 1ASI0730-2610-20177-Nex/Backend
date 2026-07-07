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

public class PropertyCommandService(
    IPropertyRepository propertyRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IPropertyCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Property>> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
    {
        var property = new Property(command);
        try
        {
            await propertyRepository.AddAsync(property, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Property>.Success(property);
        }
        catch (OperationCanceledException)
        {
            return Result<Property>.Failure(DevicesError.OperationCancelled,
                _localizer[nameof(DevicesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Property>.Failure(DevicesError.DatabaseError,
                _localizer[nameof(DevicesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Property>.Failure(DevicesError.InternalServerError,
                _localizer[nameof(DevicesError.InternalServerError)]);
        }
    }
}
