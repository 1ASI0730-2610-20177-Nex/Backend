using Electro.Corporation.Platform.Profiles.Application.CommandServices;
using Electro.Corporation.Platform.Profiles.Application.QueryServices;
using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.Queries;
using Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;
using Electro.Corporation.Platform.Profiles.Interfaces.Acl;

namespace Electro.Corporation.Platform.Profiles.Application.Acl;

/// <summary>
///     Facade for the profiles context
/// </summary>
/// <param name="profileCommandService">
///     The profile command service
/// </param>
/// <param name="profileQueryService">
///     The profile query service
/// </param>
public class ProfilesContextFacade(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService
) : IProfilesContextFacade
{
    // inheritedDoc
    public async Task<int> CreateProfile(int userId, string firstName, string lastName, string email, string street,
        string number, string city, string postalCode, string country, CancellationToken cancellationToken)
    {
        var createProfileCommand =
            new CreateProfileCommand(userId, firstName, lastName, email, street, number, city, postalCode, country);
        var result = await profileCommandService.Handle(createProfileCommand, cancellationToken);
        return result.Value?.Id ?? 0;
    }

    // inheritedDoc
    public async Task<int> FetchProfileIdByEmail(string email, CancellationToken cancellationToken)
    {
        var getProfileByEmailQuery = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(getProfileByEmailQuery, cancellationToken);
        return profile?.Id ?? 0;
    }
}
