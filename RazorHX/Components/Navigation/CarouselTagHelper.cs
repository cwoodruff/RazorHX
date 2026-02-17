using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Components.Imagery;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Navigation;

/// <summary>
/// Renders an accessible carousel/slider with optional navigation arrows,
/// pagination dots, autoplay, touch/mouse dragging, and loop support.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-carousel rhx-loop="true" rhx-pagination="true"&gt;
///     &lt;rhx-carousel-item&gt;Slide 1&lt;/rhx-carousel-item&gt;
///     &lt;rhx-carousel-item&gt;Slide 2&lt;/rhx-carousel-item&gt;
/// &lt;/rhx-carousel&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-carousel")]
public class CarouselTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "carousel";

    /// <summary>
    /// Whether the carousel loops (wraps around at the ends).
    /// </summary>
    [HtmlAttributeName("rhx-loop")]
    public bool Loop { get; set; }

    /// <summary>
    /// Whether to show previous/next navigation arrows. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-navigation")]
    public bool Navigation { get; set; } = true;

    /// <summary>
    /// Whether to show pagination dots. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-pagination")]
    public bool Pagination { get; set; } = true;

    /// <summary>
    /// Whether the carousel auto-advances slides.
    /// </summary>
    [HtmlAttributeName("rhx-autoplay")]
    public bool Autoplay { get; set; }

    /// <summary>
    /// Autoplay interval in milliseconds. Default: 5000.
    /// </summary>
    [HtmlAttributeName("rhx-autoplay-interval")]
    public int AutoplayInterval { get; set; } = 5000;

    /// <summary>
    /// Number of slides visible at once. Default: 1.
    /// </summary>
    [HtmlAttributeName("rhx-slides-per-page")]
    public int SlidesPerPage { get; set; } = 1;

    /// <summary>
    /// Number of slides to advance per navigation action. Default: 1.
    /// </summary>
    [HtmlAttributeName("rhx-slides-per-move")]
    public int SlidesPerMove { get; set; } = 1;

    /// <summary>
    /// Carousel orientation: horizontal (default) or vertical.
    /// </summary>
    [HtmlAttributeName("rhx-orientation")]
    public string Orientation { get; set; } = "horizontal";

    /// <summary>
    /// Whether to enable mouse dragging. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-mouse-dragging")]
    public bool MouseDragging { get; set; } = true;

    /// <summary>
    /// Accessible label for the carousel region.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new CarouselTagHelper with URL generation support.
    /// </summary>
    public CarouselTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Set up slide counter for child items (tests may pre-populate)
        if (!context.Items.ContainsKey("CarouselSlideCount"))
            context.Items["CarouselSlideCount"] = new List<int>();
        var slideList = (List<int>)context.Items["CarouselSlideCount"];

        context.Items[typeof(CarouselTagHelper)] = this;

        // Process children — CarouselItemTagHelpers register themselves
        var childContent = await output.GetChildContentAsync();
        var slideCount = slideList.Count;

        // Render container
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var orientation = Orientation.ToLowerInvariant();
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("vertical"), orientation == "vertical");
        ApplyBaseAttributes(output, css);

        // Data attributes for JS
        output.Attributes.SetAttribute("data-rhx-carousel", "");
        output.Attributes.SetAttribute("data-rhx-slide-count", slideCount.ToString());
        if (Loop)
            output.Attributes.SetAttribute("data-rhx-loop", "true");
        if (Autoplay)
            output.Attributes.SetAttribute("data-rhx-autoplay", AutoplayInterval.ToString());
        output.Attributes.SetAttribute("data-rhx-slides-per-page", SlidesPerPage.ToString());
        output.Attributes.SetAttribute("data-rhx-slides-per-move", SlidesPerMove.ToString());
        output.Attributes.SetAttribute("data-rhx-orientation", orientation);
        if (MouseDragging)
            output.Attributes.SetAttribute("data-rhx-mouse-dragging", "true");

        // ARIA
        output.Attributes.SetAttribute("role", "region");
        output.Attributes.SetAttribute("aria-roledescription", "carousel");
        if (!string.IsNullOrWhiteSpace(AriaLabel))
            output.Attributes.SetAttribute("aria-label", AriaLabel);

        RenderHtmxAttributes(output);

        // Assemble inner HTML
        output.Content.Clear();

        // Viewport > Track > slides
        output.Content.AppendHtml($"<div class=\"{GetElementClass("viewport")}\">");
        output.Content.AppendHtml($"<div class=\"{GetElementClass("track")}\" aria-live=\"polite\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div></div>");

        // Navigation arrows
        if (Navigation && slideCount > 1)
        {
            var chevronLeft = IconRegistry.Get("chevron-left") ?? "";
            var chevronRight = IconRegistry.Get("chevron-right") ?? "";

            output.Content.AppendHtml($"<div class=\"{GetElementClass("navigation")}\">");
            output.Content.AppendHtml(
                $"<button class=\"{GetElementClass("nav-button")} {GetElementClass("nav-button")}--prev\" " +
                "type=\"button\" aria-label=\"Previous slide\">" +
                "<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" " +
                $"stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">{Enc(chevronLeft)}</svg>" +
                "</button>");
            output.Content.AppendHtml(
                $"<button class=\"{GetElementClass("nav-button")} {GetElementClass("nav-button")}--next\" " +
                "type=\"button\" aria-label=\"Next slide\">" +
                "<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" " +
                $"stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">{Enc(chevronRight)}</svg>" +
                "</button>");
            output.Content.AppendHtml("</div>");
        }

        // Pagination dots
        if (Pagination && slideCount > 1)
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("pagination")}\" role=\"tablist\">");
            for (var i = 1; i <= slideCount; i++)
            {
                var selected = i == 1 ? "true" : "false";
                var tabindex = i == 1 ? "0" : "-1";
                output.Content.AppendHtml(
                    $"<button class=\"{GetElementClass("dot")}\" role=\"tab\" " +
                    $"aria-label=\"Slide {i}\" aria-selected=\"{selected}\" tabindex=\"{tabindex}\">" +
                    "</button>");
            }
            output.Content.AppendHtml("</div>");
        }
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
