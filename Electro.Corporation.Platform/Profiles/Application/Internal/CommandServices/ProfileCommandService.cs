using Electro.Corporation.Platform.Iam.Interfaces.Acl;
using Electro.Corporation.Platform.Profiles.Application.CommandServices;
using Electro.Corporation.Platform.Profiles.Domain.Model;
using Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;
using Electro.Corporation.Platform.Profiles.Domain.Repositories;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Shared.Application.Model;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Electro.Corporation.Platform.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(
    IProfileRepository profileRepository,
    IIamContextFacade iamContextFacade,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer) : IProfileCommandService
{
    private readonly IStringLocalizer<ErrorMessages> _localizer = localizer;

    public async Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        var username = await iamContextFacade.FetchUsernameByUserId(command.UserId, cancellationToken);
        if (string.IsNullOrEmpty(username))
            return Result<Profile>.Failure(ProfilesError.UserNotFound,
                _localizer[nameof(ProfilesError.UserNotFound)]);

        var email = new EmailAddress(command.Email);
        var existingProfile = await profileRepository.FindProfileByEmailAsync(email, cancellationToken);
        if (existingProfile is not null)
            return Result<Profile>.Failure(ProfilesError.EmailAlreadyRegistered,
                _localizer[nameof(ProfilesError.EmailAlreadyRegistered), command.Email]);

        var profile = new Profile(command);
        try
        {
            await profileRepository.AddAsync(profile, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Profile>.Success(profile);
        }
        catch (OperationCanceledException)
        {
            return Result<Profile>.Failure(ProfilesError.OperationCancelled,
                _localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Profile>.Failure(ProfilesError.DatabaseError,
                _localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Profile>.Failure(ProfilesError.InternalServerError,
                _localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }
}
