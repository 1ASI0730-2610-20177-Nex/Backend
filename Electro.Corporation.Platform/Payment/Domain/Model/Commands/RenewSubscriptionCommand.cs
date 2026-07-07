namespace Electro.Corporation.Platform.Payment.Domain.Model.Commands;

public record RenewSubscriptionCommand(int SubscriptionId, string PaymentMethod);


