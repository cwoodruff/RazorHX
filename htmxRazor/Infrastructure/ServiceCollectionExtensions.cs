using htmxRazor.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for registering htmxRazor services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds htmxRazor services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddhtmxRazor(
        this IServiceCollection services,
        Action<htmxRazorOptions>? configure = null)
    {
        var options = new htmxRazorOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);

        services.AddTransient<ITagHelperComponent, htmxRazorTagHelperComponent>();

        return services;
    }
}
