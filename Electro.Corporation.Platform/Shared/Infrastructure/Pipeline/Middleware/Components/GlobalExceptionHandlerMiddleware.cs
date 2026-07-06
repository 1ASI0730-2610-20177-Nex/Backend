using System.Net.Mime;
using System.Text.Json;
using Electro.Corporation.Platform.Resources.Errors;
using Electro.Corporation.Platform.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For OperationCanceledException

namespace Electro.Corporation.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    IStringLocalizer<ErrorMessages> errorLocalizer, // Inject IStringLocalizer for error messages
    IStringLocalizer<CommonMessages> // Corrected to Commons
        commonLocalizer) // Inject IStringLocalizer for common messages like "Internal Server Error"
{
    private readonly IStringLocalizer<CommonMessages> _commonLocalizer = commonLocalizer; // Corrected to Commons
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer = errorLocalizer;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Request was cancelled: {Message}", ex.Message);
            await HandleOperationCanceledExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleGenericExceptionAsync(context, ex);
        }
    }


    private async Task HandleOperationCanceledExceptionAsync(HttpContext context, OperationCanceledException exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status409Conflict; // Or 204 No Content if appropriate

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = _errorLocalizer["OperationCancelled"], // Localized title
            Detail = _errorLocalizer["OperationCancelled"], // Localized detail
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

        await context.Response.WriteAsync(result);
    }
    
    private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = _commonLocalizer["InternalServerError"], // Localized title
            Detail = _errorLocalizer["GenericError"], // Localized generic error message
            Instance = context.Request.Path
        };

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, jsonOptions);

        await context.Response.WriteAsync(result);
    }
}