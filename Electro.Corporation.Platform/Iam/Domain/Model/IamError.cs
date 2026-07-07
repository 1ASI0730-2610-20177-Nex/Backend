namespace Electro.Corporation.Platform.Iam.Domain.Model;

public enum IamError
{
    None,
    UserNotFound,
    UsernameAlreadyTaken,
    InvalidCredentials,
    OperationCancelled,
    DatabaseError,
    InternalServerError,
    ExternalServiceError
}
