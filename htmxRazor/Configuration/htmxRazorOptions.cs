namespace htmxRazor.Configuration;

/// <summary>
/// Configuration options for htmxRazor.
/// </summary>
public sealed class htmxRazorOptions
{
    /// <summary>
    /// The default theme to apply. Valid values: "light", "dark".
    /// Default: "light".
    /// </summary>
    public string DefaultTheme { get; set; } = "light";

    /// <summary>
    /// Whether to enable per-component CSS isolation.
    /// When true, only the CSS for components actually used on a page is loaded.
    /// Default: false (all component CSS is bundled).
    /// </summary>
    public bool EnableCssIsolation { get; set; }

    /// <summary>
    /// Base URL for loading htmx from a CDN.
    /// Default: "https://unpkg.com" (used as https://unpkg.com/htmx.org@version).
    /// </summary>
    public string CdnBaseUrl { get; set; } = "https://unpkg.com";

    /// <summary>
    /// Whether to auto-inject the htmx script tag into the page head.
    /// Set to false if you manage the htmx script yourself.
    /// Default: true.
    /// </summary>
    public bool IncludeHtmxScript { get; set; } = true;
}
