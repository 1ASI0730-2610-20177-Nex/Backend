using Electro.Corporation.Platform.Shared.Domain.Model.Entities;

namespace Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;

public partial class Profile : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
