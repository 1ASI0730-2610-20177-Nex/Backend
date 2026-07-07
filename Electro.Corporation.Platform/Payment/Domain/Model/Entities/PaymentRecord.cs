using Electro.Corporation.Platform.Payment.Domain.Model;

namespace Electro.Corporation.Platform.Payment.Domain.Model.Entities;

public class PaymentRecord
{
    public PaymentRecord()
    {
    }

    public PaymentRecord(int subscriptionId, decimal amount, PaymentStatus status, string paymentMethod)
    {
        SubscriptionId = subscriptionId;
        Amount = amount;
        PaidAt = DateTime.UtcNow;
        Status = status;
        PaymentMethod = paymentMethod;
    }

    public int Id { get; set; }
    public int SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public PaymentStatus Status { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
}

