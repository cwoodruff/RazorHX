using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Infrastructure;

/// <summary>
/// Abstract base class for all RazorHX tag helpers.
/// Provides shared functionality for CSS class building, ARIA attributes, and htmx integration.
/// </summary>
public abstract class RazorHXTagHelperBase : TagHelper
{
    /// <summary>
    /// The BEM block name for this component (e.g., "button", "input", "card").
    /// </summary>
    protected abstract string BlockName { get; }

    /// <summary>
    /// The CSS prefix used for all RazorHX components.
    /// </summary>
    protected const string CssPrefix = "rhx-";

    /// <summary>
    /// Optional additional CSS classes to apply to the root element.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <summary>
    /// Optional element ID.
    /// </summary>
    [HtmlAttributeName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Whether the component is hidden.
    /// </summary>
    [HtmlAttributeName("hidden")]
    public bool Hidden { get; set; }

    /// <summary>
    /// Creates a new CssClassBuilder pre-initialized with this component's block class.
    /// </summary>
    protected CssClassBuilder CreateCssBuilder() => new CssClassBuilder(GetBlockClass());

    /// <summary>
    /// Gets the full BEM block class name (e.g., "rhx-button").
    /// </summary>
    protected string GetBlockClass() => $"{CssPrefix}{BlockName}";

    /// <summary>
    /// Gets a BEM modifier class name (e.g., "rhx-button--primary").
    /// </summary>
    protected string GetModifierClass(string modifier) => $"{CssPrefix}{BlockName}--{modifier}";

    /// <summary>
    /// Gets a BEM element class name (e.g., "rhx-button__icon").
    /// </summary>
    protected string GetElementClass(string element) => $"{CssPrefix}{BlockName}__{element}";

    /// <summary>
    /// Applies the computed CSS classes and common attributes to the tag output.
    /// </summary>
    protected void ApplyBaseAttributes(TagHelperOutput output, CssClassBuilder cssBuilder)
    {
        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            cssBuilder.Add(CssClass);
        }

        output.Attributes.SetAttribute("class", cssBuilder.Build());

        if (!string.IsNullOrWhiteSpace(Id))
        {
            output.Attributes.SetAttribute("id", Id);
        }

        if (Hidden)
        {
            output.Attributes.SetAttribute("hidden", "hidden");
        }
    }
}
