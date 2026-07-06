
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseSnakeCaseNamingConvention();
    }
}
