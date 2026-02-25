using System.Collections.Concurrent;

namespace htmxRazor.Components.Imagery;

/// <summary>
/// Static registry of SVG icon paths for use with <c>&lt;rhx-icon&gt;</c>.
/// Ships with ~40 essential UI icons. Users can register custom icons via
/// <see cref="Register"/> at application startup.
/// </summary>
/// <remarks>
/// <para>
/// All built-in icons use a 24×24 viewBox with <c>fill="none" stroke="currentColor" stroke-width="2"</c>.
/// Custom icons should follow the same convention for consistency.
/// </para>
/// <para>
/// To add custom icons, call <c>IconRegistry.Register("my-icon", "&lt;path d=\"...\" /&gt;")</c>
/// in your <c>Program.cs</c> or startup code.
/// </para>
/// </remarks>
public static class IconRegistry
{
    private static readonly ConcurrentDictionary<string, string> Icons = new(StringComparer.OrdinalIgnoreCase);

    static IconRegistry()
    {
        // ── Arrows ──
        Register("arrow-up", "<path d=\"M12 19V5\" /><path d=\"M5 12l7-7 7 7\" />");
        Register("arrow-down", "<path d=\"M12 5v14\" /><path d=\"M19 12l-7 7-7-7\" />");
        Register("arrow-left", "<path d=\"M19 12H5\" /><path d=\"M12 19l-7-7 7-7\" />");
        Register("arrow-right", "<path d=\"M5 12h14\" /><path d=\"M12 5l7 7-7 7\" />");

        // ── Chevrons ──
        Register("chevron-up", "<path d=\"M18 15l-6-6-6 6\" />");
        Register("chevron-down", "<path d=\"M6 9l6 6 6-6\" />");
        Register("chevron-left", "<path d=\"M15 18l-6-6 6-6\" />");
        Register("chevron-right", "<path d=\"M9 18l6-6-6-6\" />");

        // ── Actions ──
        Register("check", "<path d=\"M20 6L9 17l-5-5\" />");
        Register("x", "<path d=\"M18 6L6 18\" /><path d=\"M6 6l12 12\" />");
        Register("plus", "<path d=\"M12 5v14\" /><path d=\"M5 12h14\" />");
        Register("minus", "<path d=\"M5 12h14\" />");
        Register("search", "<circle cx=\"11\" cy=\"11\" r=\"8\" /><path d=\"M21 21l-4.35-4.35\" />");
        Register("edit", "<path d=\"M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7\" /><path d=\"M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z\" />");
        Register("trash", "<path d=\"M3 6h18\" /><path d=\"M19 6v14a2 2 0 01-2 2H7a2 2 0 01-2-2V6\" /><path d=\"M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2\" />");
        Register("copy", "<rect x=\"9\" y=\"9\" width=\"13\" height=\"13\" rx=\"2\" /><path d=\"M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1\" />");
        Register("download", "<path d=\"M21 15v4a2 2 0 01-2 2H5a2 2 0 01-2-2v-4\" /><path d=\"M7 10l5 5 5-5\" /><path d=\"M12 15V3\" />");
        Register("upload", "<path d=\"M21 15v4a2 2 0 01-2 2H5a2 2 0 01-2-2v-4\" /><path d=\"M17 8l-5-5-5 5\" /><path d=\"M12 3v12\" />");
        Register("refresh", "<path d=\"M23 4v6h-6\" /><path d=\"M1 20v-6h6\" /><path d=\"M3.51 9a9 9 0 0114.85-3.36L23 10\" /><path d=\"M20.49 15a9 9 0 01-14.85 3.36L1 14\" />");
        Register("external-link", "<path d=\"M18 13v6a2 2 0 01-2 2H5a2 2 0 01-2-2V8a2 2 0 012-2h6\" /><path d=\"M15 3h6v6\" /><path d=\"M10 14L21 3\" />");
        Register("link", "<path d=\"M10 13a5 5 0 007.54.54l3-3a5 5 0 00-7.07-7.07l-1.72 1.71\" /><path d=\"M14 11a5 5 0 00-7.54-.54l-3 3a5 5 0 007.07 7.07l1.71-1.71\" />");
        Register("filter", "<path d=\"M22 3H2l8 9.46V19l4 2v-8.54L22 3z\" />");
        Register("sort", "<path d=\"M11 5h10\" /><path d=\"M11 9h7\" /><path d=\"M11 13h4\" /><path d=\"M3 17l3 3 3-3\" /><path d=\"M6 18V4\" />");

        // ── Navigation ──
        Register("menu", "<path d=\"M3 12h18\" /><path d=\"M3 6h18\" /><path d=\"M3 18h18\" />");
        Register("home", "<path d=\"M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2V9z\" /><path d=\"M9 22V12h6v10\" />");
        Register("more-horizontal", "<circle cx=\"12\" cy=\"12\" r=\"1\" /><circle cx=\"19\" cy=\"12\" r=\"1\" /><circle cx=\"5\" cy=\"12\" r=\"1\" />");
        Register("more-vertical", "<circle cx=\"12\" cy=\"12\" r=\"1\" /><circle cx=\"12\" cy=\"5\" r=\"1\" /><circle cx=\"12\" cy=\"19\" r=\"1\" />");

        // ── Objects ──
        Register("user", "<path d=\"M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2\" /><circle cx=\"12\" cy=\"7\" r=\"4\" />");
        Register("users", "<path d=\"M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2\" /><circle cx=\"9\" cy=\"7\" r=\"4\" /><path d=\"M23 21v-2a4 4 0 00-3-3.87\" /><path d=\"M16 3.13a4 4 0 010 7.75\" />");
        Register("settings", "<circle cx=\"12\" cy=\"12\" r=\"3\" /><path d=\"M19.4 15a1.65 1.65 0 00.33 1.82l.06.06a2 2 0 010 2.83 2 2 0 01-2.83 0l-.06-.06a1.65 1.65 0 00-1.82-.33 1.65 1.65 0 00-1 1.51V21a2 2 0 01-4 0v-.09a1.65 1.65 0 00-1.08-1.51 1.65 1.65 0 00-1.82.33l-.06.06a2 2 0 01-2.83-2.83l.06-.06a1.65 1.65 0 00.33-1.82 1.65 1.65 0 00-1.51-1H3a2 2 0 010-4h.09a1.65 1.65 0 001.51-1.08 1.65 1.65 0 00-.33-1.82l-.06-.06a2 2 0 012.83-2.83l.06.06a1.65 1.65 0 001.82.33H9a1.65 1.65 0 001-1.51V3a2 2 0 014 0v.09a1.65 1.65 0 001.08 1.51 1.65 1.65 0 001.82-.33l.06-.06a2 2 0 012.83 2.83l-.06.06a1.65 1.65 0 00-.33 1.82V9a1.65 1.65 0 001.51 1H21a2 2 0 010 4h-.09a1.65 1.65 0 00-1.51 1.08z\" />");
        Register("mail", "<path d=\"M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z\" /><path d=\"M22 6l-10 7L2 6\" />");
        Register("calendar", "<rect x=\"3\" y=\"4\" width=\"18\" height=\"18\" rx=\"2\" /><path d=\"M16 2v4\" /><path d=\"M8 2v4\" /><path d=\"M3 10h18\" />");
        Register("clock", "<circle cx=\"12\" cy=\"12\" r=\"10\" /><path d=\"M12 6v6l4 2\" />");
        Register("file", "<path d=\"M13 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V9z\" /><path d=\"M13 2v7h7\" />");
        Register("folder", "<path d=\"M22 19a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2h5l2 3h9a2 2 0 012 2v11z\" />");
        Register("image", "<rect x=\"3\" y=\"3\" width=\"18\" height=\"18\" rx=\"2\" /><circle cx=\"8.5\" cy=\"8.5\" r=\"1.5\" /><path d=\"M21 15l-5-5L5 21\" />");
        Register("heart", "<path d=\"M20.84 4.61a5.5 5.5 0 00-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 00-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 000-7.78z\" />");
        Register("star", "<path d=\"M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z\" />");
        Register("bell", "<path d=\"M18 8A6 6 0 006 8c0 7-3 9-3 9h18s-3-2-3-9\" /><path d=\"M13.73 21a2 2 0 01-3.46 0\" />");

        // ── Status ──
        Register("info", "<circle cx=\"12\" cy=\"12\" r=\"10\" /><path d=\"M12 16v-4\" /><path d=\"M12 8h.01\" />");
        Register("alert-circle", "<circle cx=\"12\" cy=\"12\" r=\"10\" /><path d=\"M12 8v4\" /><path d=\"M12 16h.01\" />");
        Register("alert-triangle", "<path d=\"M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z\" /><path d=\"M12 9v4\" /><path d=\"M12 17h.01\" />");
        Register("check-circle", "<path d=\"M22 11.08V12a10 10 0 11-5.93-9.14\" /><path d=\"M22 4L12 14.01l-3-3\" />");
        Register("x-circle", "<circle cx=\"12\" cy=\"12\" r=\"10\" /><path d=\"M15 9l-6 6\" /><path d=\"M9 9l6 6\" />");

        // ── Media ──
        Register("play", "<polygon points=\"5 3 19 12 5 21 5 3\" />");
        Register("pause", "<rect x=\"6\" y=\"4\" width=\"4\" height=\"16\" /><rect x=\"14\" y=\"4\" width=\"4\" height=\"16\" />");
        Register("eye", "<path d=\"M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z\" /><circle cx=\"12\" cy=\"12\" r=\"3\" />");
        Register("eye-off", "<path d=\"M17.94 17.94A10.07 10.07 0 0112 20c-7 0-11-8-11-8a18.45 18.45 0 015.06-5.94\" /><path d=\"M9.9 4.24A9.12 9.12 0 0112 4c7 0 11 8 11 8a18.5 18.5 0 01-2.16 3.19\" /><path d=\"M14.12 14.12a3 3 0 11-4.24-4.24\" /><path d=\"M1 1l22 22\" />");
        Register("lock", "<rect x=\"3\" y=\"11\" width=\"18\" height=\"11\" rx=\"2\" /><path d=\"M7 11V7a5 5 0 0110 0v4\" />");
        Register("unlock", "<rect x=\"3\" y=\"11\" width=\"18\" height=\"11\" rx=\"2\" /><path d=\"M7 11V7a5 5 0 019.9-1\" />");
        Register("globe", "<circle cx=\"12\" cy=\"12\" r=\"10\" /><path d=\"M2 12h20\" /><path d=\"M12 2a15.3 15.3 0 014 10 15.3 15.3 0 01-4 10 15.3 15.3 0 01-4-10 15.3 15.3 0 014-10z\" />");
    }

    /// <summary>
    /// Registers a custom icon or overrides a built-in icon.
    /// </summary>
    /// <param name="name">The icon name (case-insensitive).</param>
    /// <param name="svgContent">The inner SVG content (paths, circles, etc.).</param>
    public static void Register(string name, string svgContent)
    {
        Icons[name] = svgContent;
    }

    /// <summary>
    /// Returns the SVG inner content for the given icon name, or null if not found.
    /// </summary>
    /// <param name="name">The icon name (case-insensitive).</param>
    /// <returns>The SVG path data, or null.</returns>
    public static string? Get(string name)
    {
        return Icons.TryGetValue(name, out var content) ? content : null;
    }

    /// <summary>
    /// Returns true if the specified icon is registered.
    /// </summary>
    /// <param name="name">The icon name (case-insensitive).</param>
    public static bool Has(string name)
    {
        return Icons.ContainsKey(name);
    }

    /// <summary>
    /// Returns all registered icon names.
    /// </summary>
    public static IEnumerable<string> GetNames()
    {
        return Icons.Keys;
    }
}
