using Electro.Corporation.Platform.Profiles.Application.QueryServices;
using Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Profiles.Domain.Model.Queries;
using Electro.Corporation.Platform.Profiles.Domain.Repositories;

namespace Electro.Corporation.Platform.Profiles.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.ListAsync(cancellationToken);
    }

    public async Task<Profile?> Handle(GetProfileByEmailQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindProfileByEmailAsync(query.Email, cancellationToken);
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindByIdAsync(query.ProfileId, cancellationToken);
    }

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query, CancellationToken cancellationToken)
    {
        return await profileRepository.FindProfileByUserIdAsync(query.UserId, cancellationToken);
    }
}
