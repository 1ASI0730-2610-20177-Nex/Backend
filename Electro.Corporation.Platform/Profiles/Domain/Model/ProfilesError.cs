namespace Electro.Corporation.Platform.Profiles.Domain.Model;

public enum ProfilesError
{
    None,
    ProfileNotFound,
    UserNotFound,
    EmailAlreadyRegistered,
    InvalidProfileData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
