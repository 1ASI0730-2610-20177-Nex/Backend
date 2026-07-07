using Electro.Corporation.Platform.Shared.Domain.Model.Entities;

namespace Electro.Corporation.Platform.Iam.Domain.Model.Aggregates;

public partial class User : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
