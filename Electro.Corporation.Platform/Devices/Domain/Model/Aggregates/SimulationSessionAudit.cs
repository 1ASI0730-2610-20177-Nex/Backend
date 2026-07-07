using Electro.Corporation.Platform.Shared.Domain.Model.Entities;

namespace Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;

public partial class SimulationSession : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
