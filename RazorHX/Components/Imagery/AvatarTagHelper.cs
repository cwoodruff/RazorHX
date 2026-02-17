using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Imagery;

/// <summary>
/// Renders a user avatar with image or initials fallback.
/// Supports circle, square, and rounded shapes with configurable sizes.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-avatar rhx-image="/photo.jpg" rhx-label="Jane Doe" /&gt;
/// &lt;rhx-avatar rhx-initials="JD" rhx-label="Jane Doe" rhx-shape="square" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-avatar")]
public class AvatarTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "avatar";

    /// <summary>
    /// The image URL. When set, renders an <c>&lt;img&gt;</c> element.
    /// </summary>
    [HtmlAttributeName("rhx-image")]
    public string? Image { get; set; }

    /// <summary>
    /// Accessible label for the avatar (used as aria-label).
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Initials to display when no image is provided (e.g., "JD").
    /// </summary>
    [HtmlAttributeName("rhx-initials")]
    public string? Initials { get; set; }

    /// <summary>
    /// Avatar shape: circle (default), square, or rounded.
    /// </summary>
    [HtmlAttributeName("rhx-shape")]
    public string Shape { get; set; } = "circle";

    /// <summary>
    /// Avatar size: small, medium (default), large, or a custom CSS value (e.g., "4rem").
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string Size { get; set; } = "medium";

    /// <summary>
    /// Image loading strategy: eager or lazy (default: lazy).
    /// </summary>
    [HtmlAttributeName("rhx-loading")]
    public string Loading { get; set; } = "lazy";

    /// <summary>
    /// Creates a new AvatarTagHelper with URL generation support.
    /// </summary>
    public AvatarTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var shape = Shape.ToLowerInvariant();
        var size = Size.ToLowerInvariant();
        var isNamedSize = size is "small" or "medium" or "large";

        var css = CreateCssBuilder()
            .Add(GetModifierClass(shape));
        if (isNamedSize)
            css.Add(GetModifierClass(size));
        ApplyBaseAttributes(output, css);

        // Custom size via CSS variable
        if (!isNamedSize)
            output.Attributes.SetAttribute("style", $"--rhx-avatar-size: {Enc(Size)}");

        output.Attributes.SetAttribute("role", "img");
        if (!string.IsNullOrWhiteSpace(Label))
            output.Attributes.SetAttribute("aria-label", Label);

        RenderHtmxAttributes(output);

        // Content: image or initials
        output.Content.Clear();
        if (!string.IsNullOrWhiteSpace(Image))
        {
            output.Content.AppendHtml(
                $"<img class=\"{GetElementClass("image")}\" src=\"{Enc(Image)}\" alt=\"\" loading=\"{Enc(Loading)}\" />");
        }
        else if (!string.IsNullOrWhiteSpace(Initials))
        {
            var hash = GetInitialsHash(Initials);
            output.Content.AppendHtml(
                $"<span class=\"{GetElementClass("initials")}\" data-rhx-hash=\"{hash}\">{Enc(Initials)}</span>");
        }
    }

    /// <summary>
    /// Generates a simple hash from initials to deterministically vary background colors.
    /// Returns a value 0-7 used as a CSS modifier (data-rhx-hash).
    /// </summary>
    public static int GetInitialsHash(string initials)
    {
        var hash = 0;
        foreach (var c in initials)
            hash = (hash * 31 + c) & 0x7FFFFFFF;
        return hash % 8;
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
