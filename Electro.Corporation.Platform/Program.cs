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
/*
using Electro.Corporation.Platform.Profile.Application.CommandServices;
using Electro.Corporation.Platform.Profile.Application.Internal.CommandServices;
using Electro.Corporation.Platform.Profile.Application.Internal.QueryServices;
using Electro.Corporation.Platform.Profile.Application.QueryServices;
using Electro.Corporation.Platform.Profile.Domain.Repositories;
using Electro.Corporation.Platform.Profile.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Electro.Corporation.Platform.Profile.Resources;
*/
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
//builder.Services.AddSingleton<IStringLocalizer<ProfileMessages>, StringLocalizer<ProfileMessages>>();
builder.Services.AddSingleton<IStringLocalizer<DevicesMessages>, StringLocalizer<DevicesMessages>>();
builder.Services.AddSingleton<IStringLocalizer<AnalyticsMessages>, StringLocalizer<AnalyticsMessages>>();

builder.Services.AddSingleton<ProblemDetailsFactory>();

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

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Profile Bounded Context
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserCommandService, UserCommandService>();
//builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// Devices Bounded Context
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IHomeCommandService, HomeCommandService>();
builder.Services.AddScoped<IHomeQueryService, HomeQueryService>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();

// Analytics Bounded Context
builder.Services.AddScoped<IConsumptionRepository, ConsumptionRepository>();
builder.Services.AddScoped<IConsumptionCommandService, ConsumptionCommandService>();
builder.Services.AddScoped<IConsumptionQueryService, ConsumptionQueryService>();

builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
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

app.MapControllers();

app.Run();