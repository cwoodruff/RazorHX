using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using RazorHX.Configuration;

namespace RazorHX.Infrastructure;

/// <summary>
/// Extension methods for configuring the RazorHX middleware pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds RazorHX middleware to serve embedded CSS/JS assets from the /_rhx/ path prefix.
    /// </summary>
    public static IApplicationBuilder UseRazorHX(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<RazorHXOptions>() ?? new RazorHXOptions();

        var embeddedProvider = new EmbeddedFileProvider(
            typeof(ApplicationBuilderExtensions).Assembly,
            "RazorHX.Assets");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = embeddedProvider,
            RequestPath = new PathString("/_rhx"),
            ContentTypeProvider = new FileExtensionContentTypeProvider(
                new Dictionary<string, string>
                {
                    { ".css", "text/css" },
                    { ".js", "application/javascript" },
                    { ".map", "application/json" }
                })
        });

        return app;
    }
}
