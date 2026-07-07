using Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.Queries;
using Electro.Corporation.Platform.Shared.Application.Model;

namespace Electro.Corporation.Platform.Profiles.Application.CommandServices;

public interface IProfileCommandService
{
    Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken);
    Task<Result<Profile>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken);
    Task<Result<Profile>> Handle(UpdateProfilePreferencesCommand command, CancellationToken cancellationToken);
}
