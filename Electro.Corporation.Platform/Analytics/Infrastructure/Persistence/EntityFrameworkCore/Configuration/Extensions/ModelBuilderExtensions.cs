using Electro.Corporation.Platform.Analytics.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
  public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
  {
    builder.Entity<Consumption>().HasKey(c => c.Id);
    builder.Entity<Consumption>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Entity<Consumption>().Property(c => c.DeviceId).IsRequired();
    builder.Entity<Consumption>().Property(c => c.Kwh).IsRequired().HasPrecision(18, 4);
    builder.Entity<Consumption>().Property(c => c.Date).IsRequired();

    builder.Entity<Alert>().HasKey(a => a.Id);
    builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Entity<Alert>().Property(a => a.UserId).IsRequired();
    builder.Entity<Alert>().Property(a => a.PropertyId).IsRequired();
    builder.Entity<Alert>().Property(a => a.Title).IsRequired().HasMaxLength(200);
    builder.Entity<Alert>().Property(a => a.Message).IsRequired().HasMaxLength(1000);
    builder.Entity<Alert>().Property(a => a.IsRead).IsRequired();
    builder.Entity<Alert>().Property(a => a.CreatedAt).IsRequired();

    builder.Entity<Report>().HasKey(r => r.Id);
    builder.Entity<Report>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Entity<Report>().Property(r => r.PropertyId).IsRequired();
    builder.Entity<Report>().Property(r => r.Title).IsRequired().HasMaxLength(200);
    builder.Entity<Report>().Property(r => r.Summary).IsRequired().HasMaxLength(2000);
    builder.Entity<Report>().Property(r => r.GeneratedAt).IsRequired();
  }
}
