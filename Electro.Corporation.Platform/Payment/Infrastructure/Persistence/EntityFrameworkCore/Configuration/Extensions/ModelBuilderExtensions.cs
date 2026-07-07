using Electro.Corporation.Platform.Payment.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Payment.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Payment.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyPaymentConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.UserId).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Plan).IsRequired().HasConversion<string>();
        builder.Entity<Subscription>().Property(s => s.Status).IsRequired().HasConversion<string>();
        builder.Entity<Subscription>().Property(s => s.StartDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.EndDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.MonthlyAmount).IsRequired().HasPrecision(18, 2);

        builder.Entity<PaymentRecord>().HasKey(p => p.Id);
        builder.Entity<PaymentRecord>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PaymentRecord>().Property(p => p.SubscriptionId).IsRequired();
        builder.Entity<PaymentRecord>().Property(p => p.Amount).IsRequired().HasPrecision(18, 2);
        builder.Entity<PaymentRecord>().Property(p => p.PaidAt).IsRequired();
        builder.Entity<PaymentRecord>().Property(p => p.Status).IsRequired().HasConversion<string>();
        builder.Entity<PaymentRecord>().Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);

        builder.Entity<PaymentRecord>()
            .HasOne<Subscription>()
            .WithMany()
            .HasForeignKey(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


