using Electro.Corporation.Platform.Iam.Interfaces.Acl;
using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Domain.Model;
using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Devices.Application.Internal.CommandServices;

public class SimulationSessionCommandService(
    ISimulationSessionRepository simulationSessionRepository,
    ISimulationActionRepository simulationActionRepository,
    IPropertyRepository propertyRepository,
    IIamContextFacade iamContextFacade,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : ISimulationSessionCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<SimulationSession>> Handle(CreateSimulationSessionCommand command,
        CancellationToken cancellationToken)
    {
        var username = await iamContextFacade.FetchUsernameByUserId(command.UserId, cancellationToken);
        if (string.IsNullOrEmpty(username))
            return Result<SimulationSession>.Failure(DevicesError.UserNotFound,
                _localizer[nameof(DevicesError.UserNotFound)]);

        var property = await propertyRepository.FindByIdAsync(command.PropertyId, cancellationToken);
        if (property is null)
            return Result<SimulationSession>.Failure(DevicesError.PropertyNotFound,
                _localizer[nameof(DevicesError.PropertyNotFound)]);

        var existingActiveSession =
            await simulationSessionRepository.FindActiveByUserIdAsync(command.UserId, cancellationToken);
        if (existingActiveSession is not null)
            return Result<SimulationSession>.Failure(DevicesError.ActiveSessionAlreadyExists,
                _localizer[nameof(DevicesError.ActiveSessionAlreadyExists)]);

        var session = new SimulationSession(command);
        try
        {
            await simulationSessionRepository.AddAsync(session, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SimulationSession>.Success(session);
        }
        catch (OperationCanceledException)
        {
            return Result<SimulationSession>.Failure(DevicesError.OperationCancelled,
                _localizer[nameof(DevicesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<SimulationSession>.Failure(DevicesError.DatabaseError,
                _localizer[nameof(DevicesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<SimulationSession>.Failure(DevicesError.InternalServerError,
                _localizer[nameof(DevicesError.InternalServerError)]);
        }
    }

    public async Task<Result<SimulationAction>> Handle(RegisterSimulationActionCommand command,
        CancellationToken cancellationToken)
    {
        var session = await simulationSessionRepository.FindByIdAsync(command.SessionId, cancellationToken);
        if (session is null)
            return Result<SimulationAction>.Failure(DevicesError.SessionNotFound,
                _localizer[nameof(DevicesError.SessionNotFound)]);

        if (!session.IsActive)
            return Result<SimulationAction>.Failure(DevicesError.SessionAlreadyEnded,
                _localizer[nameof(DevicesError.SessionAlreadyEnded)]);

        var action = new SimulationAction(command);
        try
        {
            await simulationActionRepository.AddAsync(action, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SimulationAction>.Success(action);
        }
        catch (OperationCanceledException)
        {
            return Result<SimulationAction>.Failure(DevicesError.OperationCancelled,
                _localizer[nameof(DevicesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<SimulationAction>.Failure(DevicesError.DatabaseError,
                _localizer[nameof(DevicesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<SimulationAction>.Failure(DevicesError.InternalServerError,
                _localizer[nameof(DevicesError.InternalServerError)]);
        }
    }

    public async Task<Result<SimulationSession>> Handle(EndSimulationSessionCommand command,
        CancellationToken cancellationToken)
    {
        var session = await simulationSessionRepository.FindByIdAsync(command.SessionId, cancellationToken);
        if (session is null)
            return Result<SimulationSession>.Failure(DevicesError.SessionNotFound,
                _localizer[nameof(DevicesError.SessionNotFound)]);

        if (!session.IsActive)
            return Result<SimulationSession>.Failure(DevicesError.SessionAlreadyEnded,
                _localizer[nameof(DevicesError.SessionAlreadyEnded)]);

        session.End();
        try
        {
            simulationSessionRepository.Update(session);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<SimulationSession>.Success(session);
        }
        catch (OperationCanceledException)
        {
            return Result<SimulationSession>.Failure(DevicesError.OperationCancelled,
                _localizer[nameof(DevicesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<SimulationSession>.Failure(DevicesError.DatabaseError,
                _localizer[nameof(DevicesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<SimulationSession>.Failure(DevicesError.InternalServerError,
                _localizer[nameof(DevicesError.InternalServerError)]);
        }
    }
}
