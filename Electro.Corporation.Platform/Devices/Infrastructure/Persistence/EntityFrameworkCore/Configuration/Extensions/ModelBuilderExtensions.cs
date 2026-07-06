using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
  public static void ApplyDevicesConfiguration(this ModelBuilder builder)
  {
    builder.Entity<Home>().HasKey(h => h.Id);
    builder.Entity<Home>().Property(h => h.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Entity<Home>().Property(h => h.Name).IsRequired().HasMaxLength(100);
    builder.Entity<Home>().Property(h => h.Type).IsRequired().HasMaxLength(50);
    builder.Entity<Home>().Property(h => h.UserId).IsRequired();

    builder.Entity<Device>().HasKey(d => d.Id);
    builder.Entity<Device>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Entity<Device>().Property(d => d.Name).IsRequired().HasMaxLength(100);
    builder.Entity<Device>().Property(d => d.Type).IsRequired().HasMaxLength(50);
    builder.Entity<Device>().Property(d => d.PowerWatts).IsRequired();
    builder.Entity<Device>().Property(d => d.Status).IsRequired().HasMaxLength(50);
    builder.Entity<Device>().Property(d => d.HomeId).IsRequired();

    builder.Entity<Device>()
      .HasOne<Home>()
      .WithMany()
      .HasForeignKey(d => d.HomeId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
