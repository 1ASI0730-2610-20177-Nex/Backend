using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Commands;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Interfaces.Rest.Resources;

namespace Electro.Corporation.Platform.Devices.Interfaces.Rest.Transform;

public static class SimulationSessionResourceFromEntityAssembler
{
    public static SimulationSessionResource ToResourceFromEntity(SimulationSession session,
        IEnumerable<SimulationAction> actions)
    {
        return new SimulationSessionResource(
            session.Id,
            session.UserId,
            session.Status.ToString(),
            session.StartedAt,
            session.EndedAt,
            session.PropertyId,
            actions.Select(SimulationActionResourceFromEntityAssembler.ToResourceFromEntity));
    }
}

public static class SimulationActionResourceFromEntityAssembler
{
    public static SimulationActionResource ToResourceFromEntity(SimulationAction action)
    {
        return new SimulationActionResource(
            action.Id,
            action.SessionId,
            action.ActionType,
            action.Description,
            action.CreatedAt);
    }
}

public static class CreateSimulationSessionCommandFromResourceAssembler
{
    public static CreateSimulationSessionCommand ToCommandFromResource(CreateSimulationSessionResource resource)
    {
        return new CreateSimulationSessionCommand(resource.UserId, resource.PropertyId);
    }
}

public static class RegisterSimulationActionCommandFromResourceAssembler
{
    public static RegisterSimulationActionCommand ToCommandFromResource(int sessionId,
        CreateSimulationActionResource resource)
    {
        return new RegisterSimulationActionCommand(sessionId, resource.ActionType, resource.Description);
    }
}
