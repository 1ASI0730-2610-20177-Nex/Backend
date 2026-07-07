namespace Electro.Corporation.Platform.Devices.Domain.Model;

public enum DevicesError
{
    None,
    PropertyNotFound,
    SpaceNotFound,
    DeviceNotFound,
    SessionNotFound,
    ActiveSessionNotFound,
    SessionAlreadyEnded,
    ActiveSessionAlreadyExists,
    UserNotFound,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
