using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Interfaces.Rest.Resources;

public record PaymentResource(
    int Id,
    int SubscriptionId,
    decimal Amount,
    DateTime PaidAt,
    PaymentStatus Status,
    string PaymentMethod);


