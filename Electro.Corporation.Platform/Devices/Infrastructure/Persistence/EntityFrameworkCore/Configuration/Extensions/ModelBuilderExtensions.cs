using Electro.Corporation.Platform.Devices.Domain.Model.Aggregates;
using Electro.Corporation.Platform.Devices.Domain.Model.Entities;
using Electro.Corporation.Platform.Devices.Domain.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyDevicesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Property>().HasKey(p => p.Id);
        builder.Entity<Property>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Property>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Property>().Property(p => p.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Property>().Property(p => p.UserId).IsRequired();

        builder.Entity<Space>().HasKey(s => s.Id);
        builder.Entity<Space>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Space>().Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Space>().Property(s => s.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Space>().Property(s => s.PropertyId).IsRequired();
        builder.Entity<Space>()
            .HasOne<Property>()
            .WithMany()
            .HasForeignKey(s => s.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Device>().HasKey(d => d.Id);
        builder.Entity<Device>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Device>().Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Device>().Property(d => d.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Device>().Property(d => d.PowerWatts).IsRequired();
        builder.Entity<Device>().Property(d => d.Status).IsRequired().HasMaxLength(50);
        builder.Entity<Device>().Property(d => d.SpaceId).IsRequired();
        builder.Entity<Device>()
            .HasOne<Space>()
            .WithMany()
            .HasForeignKey(d => d.SpaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SimulationSession>().HasKey(s => s.Id);
        builder.Entity<SimulationSession>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SimulationSession>().Property(s => s.UserId).IsRequired();
        builder.Entity<SimulationSession>().Property(s => s.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Entity<SimulationSession>().Property(s => s.StartedAt).IsRequired();
        builder.Entity<SimulationSession>().Property(s => s.EndedAt).IsRequired(false);
        builder.Entity<SimulationSession>().Property(s => s.PropertyId).IsRequired();

        builder.Entity<SimulationAction>().HasKey(a => a.Id);
        builder.Entity<SimulationAction>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SimulationAction>().Property(a => a.SessionId).IsRequired();
        builder.Entity<SimulationAction>().Property(a => a.ActionType).IsRequired().HasMaxLength(100);
        builder.Entity<SimulationAction>().Property(a => a.Description).IsRequired().HasMaxLength(500);
        builder.Entity<SimulationAction>().Property(a => a.CreatedAt).IsRequired();
        builder.Entity<SimulationAction>()
            .HasOne<SimulationSession>()
            .WithMany()
            .HasForeignKey(a => a.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
