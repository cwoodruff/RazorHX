using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using RazorHX.Configuration;

namespace RazorHX.Infrastructure;

/// <summary>
/// Extension methods for registering RazorHX services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RazorHX services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddRazorHX(
        this IServiceCollection services,
        Action<RazorHXOptions>? configure = null)
    {
        var options = new RazorHXOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);

        services.AddTransient<ITagHelperComponent, RazorHXTagHelperComponent>();

        return services;
    }
}
