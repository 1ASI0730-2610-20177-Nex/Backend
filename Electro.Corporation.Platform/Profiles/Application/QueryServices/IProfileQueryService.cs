using Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Profiles.Domain.Model.Queries;

namespace Electro.Corporation.Platform.Profiles.Application.QueryServices;

public interface IProfileQueryService
{
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken);
    Task<Profile?> Handle(GetProfileByEmailQuery query, CancellationToken cancellationToken);
    Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken);
    Task<Profile?> Handle(GetProfileByUserIdQuery query, CancellationToken cancellationToken);
}
