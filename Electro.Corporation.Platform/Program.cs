using Electro.Corporation.Platform.Analytics.Application.CommandServices;
using Electro.Corporation.Platform.Analytics.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Analytics.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Analytics.Application.QueryServices;
using Electro.Corporation.Platform.Analytics.Domain.Repositories;
using Electro.Corporation.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Analytics.Resources;
using Electro.Corporation.Platform.Devices.Application.CommandServices;
using Electro.Corporation.Platform.Devices.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Devices.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Devices.Application.QueryServices;
using Electro.Corporation.Platform.Devices.Domain.Repositories;
using Electro.Corporation.Platform.Devices.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Devices.Resources;
using Electro.Corporation.Platform.Iam.Application.Acl;
using Electro.Corporation.Platform.Iam.Application.CommandServices;
using Electro.Corporation.Platform.Iam.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Iam.Application.Internal.OutboundServices;
using Electro.Corporation.Platform.Iam.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Iam.Application.QueryServices;
using Electro.Corporation.Platform.Iam.Domain.Repositories;
using Electro.Corporation.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using Electro.Corporation.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using Electro.Corporation.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Electro.Corporation.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using Electro.Corporation.Platform.Iam.Interfaces.Acl;
using Electro.Corporation.Platform.Iam.Resources;
using Electro.Corporation.Platform.Payment.Application.CommandServices;
using Electro.Corporation.Platform.Payment.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Payment.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Payment.Application.QueryServices;
using Electro.Corporation.Platform.Payment.Domain.Repositories;
using Electro.Corporation.Platform.Payment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Payment.Resources;
using Electro.Corporation.Platform.Profiles.Application.Acl;
using Electro.Corporation.Platform.Profiles.Application.CommandServices;
using Electro.Corporation.Platform.Profiles.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Profiles.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Profiles.Application.QueryServices;
using Electro.Corporation.Platform.Profiles.Domain.Repositories;
using Electro.Corporation.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Profiles.Interfaces.Acl;
using Electro.Corporation.Platform.Profiles.Resources;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Resources.Shared;
using Electro.Corporation.Platform.Shared.Domain.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Electro.Corporation.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using ProblemDetailsFactory = Electro.Corporation.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<IStringLocalizer<IamMessages>, StringLocalizer<IamMessages>>();
builder.Services.AddSingleton<IStringLocalizer<ProfilesMessages>, StringLocalizer<ProfilesMessages>>();
builder.Services.AddSingleton<IStringLocalizer<DevicesMessages>, StringLocalizer<DevicesMessages>>();
builder.Services.AddSingleton<IStringLocalizer<AnalyticsMessages>, StringLocalizer<AnalyticsMessages>>();
builder.Services.AddSingleton<IStringLocalizer<PaymentMessages>, StringLocalizer<PaymentMessages>>();

builder.Services.AddSingleton<ProblemDetailsFactory>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Electro.Corporation.Platform",
            Version = "v1",
            Description = "Electro Corporation Platform API",
            Contact = new OpenApiContact
            {
                Name = "Electro Corporation",
                Email = "contact@electro.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });
    options.EnableAnnotations();
});

// Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

// Profiles Bounded Context
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// Devices Bounded Context (properties, spaces, devices, simulation)
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<ISpaceRepository, SpaceRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<ISimulationSessionRepository, SimulationSessionRepository>();
builder.Services.AddScoped<ISimulationActionRepository, SimulationActionRepository>();
builder.Services.AddScoped<IPropertyCommandService, PropertyCommandService>();
builder.Services.AddScoped<IPropertyQueryService, PropertyQueryService>();
builder.Services.AddScoped<ISpaceCommandService, SpaceCommandService>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();
builder.Services.AddScoped<ISimulationSessionCommandService, SimulationSessionCommandService>();
builder.Services.AddScoped<ISimulationSessionQueryService, SimulationSessionQueryService>();

// Analytics Bounded Context
builder.Services.AddScoped<IConsumptionRepository, ConsumptionRepository>();
builder.Services.AddScoped<IConsumptionCommandService, ConsumptionCommandService>();
builder.Services.AddScoped<IConsumptionQueryService, ConsumptionQueryService>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IMetricsQueryService, MetricsQueryService>();

// Payment Bounded Context
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    // Ensure all existing properties have at least one default space
    var propertiesWithoutSpaces = context.Set<Electro.Corporation.Platform.Devices.Domain.Model.Entities.Property>()
        .GroupJoin(
            context.Set<Electro.Corporation.Platform.Devices.Domain.Model.Entities.Space>(),
            p => p.Id,
            s => s.PropertyId,
            (p, s) => new { Property = p, Spaces = s }
        )
        .Where(x => !x.Spaces.Any())
        .Select(x => x.Property)
        .ToList();

    foreach (var property in propertiesWithoutSpaces)
    {
        context.Set<Electro.Corporation.Platform.Devices.Domain.Model.Entities.Space>().Add(
            new Electro.Corporation.Platform.Devices.Domain.Model.Entities.Space
            {
                Name = "General",
                Type = "General",
                PropertyId = property.Id
            });
    }

    if (propertiesWithoutSpaces.Any())
    {
        context.SaveChanges();
    }
}

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseRequestAuthorization();

app.MapControllers();

app.Run();
