using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders a styled file drop zone with drag-and-drop support, file list display,
/// size validation, and automatic multipart encoding for htmx uploads.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-file-input name="avatar" rhx-label="Profile Photo" rhx-accept="image/*" /&gt;
///
/// &lt;rhx-file-input rhx-for="Documents" rhx-multiple="true" rhx-max-file-size="5242880"
///                   hx-post="/upload" hx-trigger="change" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-file-input")]
public class FileInputTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "file-input";

    // ──────────────────────────────────────────────
    //  File input-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Accepted file types (e.g., "image/*", ".pdf,.doc"). Maps to the accept attribute.</summary>
    [HtmlAttributeName("rhx-accept")]
    public string? Accept { get; set; }

    /// <summary>Whether to allow multiple file selection. Default: false.</summary>
    [HtmlAttributeName("rhx-multiple")]
    public bool Multiple { get; set; }

    /// <summary>Maximum file size in bytes. Validated client-side. Default: null (no limit).</summary>
    [HtmlAttributeName("rhx-max-file-size")]
    public long? MaxFileSize { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public FileInputTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var resolvedName = ResolveName();
        var resolvedId = ResolveId();
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-file-input", "");
        if (MaxFileSize.HasValue)
            output.Attributes.SetAttribute("data-rhx-max-size", MaxFileSize.Value.ToString());

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label
        sb.Append(BuildLabelHtml(resolvedId));

        // Drop zone
        sb.Append($"<label class=\"{GetElementClass("dropzone")}\" for=\"{Enc(resolvedId)}\">");

        // Upload icon
        sb.Append($"<svg class=\"{GetElementClass("icon")}\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
        sb.Append("<path d=\"M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4\"></path>");
        sb.Append("<polyline points=\"17 8 12 3 7 8\"></polyline>");
        sb.Append("<line x1=\"12\" y1=\"3\" x2=\"12\" y2=\"15\"></line>");
        sb.Append("</svg>");

        // Text
        sb.Append($"<span class=\"{GetElementClass("text")}\">");
        sb.Append("Drag &amp; drop files here, or <strong>browse</strong>");
        sb.Append("</span>");

        // Native file input (visually hidden)
        sb.Append($"<input type=\"file\" class=\"{GetElementClass("native")} rhx-sr-only\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        if (!string.IsNullOrEmpty(Accept))
            sb.Append($" accept=\"{Enc(Accept)}\"");
        if (Multiple) sb.Append(" multiple");
        if (Disabled) sb.Append(" disabled");
        if (resolvedRequired) sb.Append(" required");

        // ARIA
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");

        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");

        if (hasError) sb.Append(" aria-invalid=\"true\"");
        if (resolvedRequired) sb.Append(" aria-required=\"true\"");

        // htmx on native input — auto-set multipart encoding when htmx verbs present
        var htmxStr = BuildHtmxAttributeString();
        sb.Append(htmxStr);
        if (!string.IsNullOrEmpty(htmxStr) && HxEncoding == null)
            sb.Append(" hx-encoding=\"multipart/form-data\"");

        sb.Append(BuildValidationAttributeString());
        sb.Append(" />");

        sb.Append("</label>"); // close dropzone

        // File list container (populated by JS)
        sb.Append($"<div class=\"{GetElementClass("file-list")}\" aria-live=\"polite\"></div>");

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }
}
