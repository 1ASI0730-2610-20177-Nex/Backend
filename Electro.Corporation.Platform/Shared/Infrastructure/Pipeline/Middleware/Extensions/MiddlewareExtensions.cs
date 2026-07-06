using Electro.Corporation.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace Electro.Corporation.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}