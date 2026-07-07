using Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;
using Electro.Corporation.Platform.Profiles.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Profile repository implementation
/// </summary>
/// <param name="context">
///     The database context
/// </param>
public class ProfileRepository(AppDbContext context)
    : BaseRepository<Profile>(context), IProfileRepository
{
    /// <inheritdoc />
    public async Task<Profile?> FindProfileByEmailAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>().FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public async Task<Profile?> FindProfileByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }
}
