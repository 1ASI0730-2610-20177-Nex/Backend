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
  }
}
