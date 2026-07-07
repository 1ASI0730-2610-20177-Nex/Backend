namespace Electro.Corporation.Platform.Payment.Domain.Model;

public enum PaymentError
{
    None,
    SubscriptionNotFound,
    UserNotFound,
    ActiveSubscriptionAlreadyExists,
    SubscriptionAlreadyCancelled,
    InvalidPlan,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}


