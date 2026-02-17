using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace RazorHX.Infrastructure;

/// <summary>
/// Abstract base class for all RazorHX tag helpers.
/// Provides shared htmx attribute handling with ASP.NET Core route generation,
/// CSS class building utilities, and BEM naming conventions.
/// </summary>
public abstract class RazorHXTagHelperBase : TagHelper
{
    private readonly IUrlHelperFactory? _urlHelperFactory;

    /// <summary>
    /// The BEM block name for this component (e.g., "button", "input", "card").
    /// Subclasses must provide this to enable BEM class generation.
    /// </summary>
    protected abstract string BlockName { get; }

    /// <summary>
    /// CSS prefix for all RazorHX components.
    /// </summary>
    protected const string CssPrefix = "rhx-";

    // ──────────────────────────────────────────────
    //  ViewContext & URL generation
    // ──────────────────────────────────────────────

    /// <summary>
    /// The current Razor view context. Automatically injected by the framework.
    /// </summary>
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    // ──────────────────────────────────────────────
    //  Common HTML attributes
    // ──────────────────────────────────────────────

    /// <summary>
    /// Additional CSS classes to apply to the root element.
    /// Merged with the component's generated classes.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <summary>
    /// The HTML id attribute for the root element.
    /// </summary>
    [HtmlAttributeName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Whether the component is hidden.
    /// </summary>
    [HtmlAttributeName("hidden")]
    public bool Hidden { get; set; }

    // ──────────────────────────────────────────────
    //  htmx verb attributes
    // ──────────────────────────────────────────────

    /// <summary>
    /// Issues a GET request to the given URL (or generated route URL).
    /// When set to empty string and hx-page/hx-controller/hx-action are specified,
    /// the URL is generated from ASP.NET Core routing.
    /// </summary>
    [HtmlAttributeName("hx-get")]
    public string? HxGet { get; set; }

    /// <summary>
    /// Issues a POST request to the given URL (or generated route URL).
    /// </summary>
    [HtmlAttributeName("hx-post")]
    public string? HxPost { get; set; }

    /// <summary>
    /// Issues a PUT request to the given URL (or generated route URL).
    /// </summary>
    [HtmlAttributeName("hx-put")]
    public string? HxPut { get; set; }

    /// <summary>
    /// Issues a PATCH request to the given URL (or generated route URL).
    /// </summary>
    [HtmlAttributeName("hx-patch")]
    public string? HxPatch { get; set; }

    /// <summary>
    /// Issues a DELETE request to the given URL (or generated route URL).
    /// </summary>
    [HtmlAttributeName("hx-delete")]
    public string? HxDelete { get; set; }

    // ──────────────────────────────────────────────
    //  ASP.NET Core route generation
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page to generate a URL for. Used with hx-get/post/put/patch/delete
    /// when the attribute value is empty, to produce a server-routed URL.
    /// </summary>
    [HtmlAttributeName("hx-page")]
    public string? HxPage { get; set; }

    /// <summary>
    /// The MVC controller to generate a URL for.
    /// </summary>
    [HtmlAttributeName("hx-controller")]
    public string? HxController { get; set; }

    /// <summary>
    /// The MVC action to generate a URL for.
    /// </summary>
    [HtmlAttributeName("hx-action")]
    public string? HxAction { get; set; }

    /// <summary>
    /// Route parameter values for URL generation. Bound from hx-route-* attributes.
    /// Example: hx-route-id="42" produces { "id": "42" } for the route generator.
    /// </summary>
    [HtmlAttributeName("hx-route-", DictionaryAttributePrefix = "hx-route-")]
    public Dictionary<string, string> HxRouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The page handler to generate a URL for. Used with hx-page.
    /// </summary>
    [HtmlAttributeName("hx-handler")]
    public string? HxHandler { get; set; }

    // ──────────────────────────────────────────────
    //  htmx behavior attributes
    // ──────────────────────────────────────────────

    /// <summary>
    /// CSS selector of the target element for the htmx swap.
    /// </summary>
    [HtmlAttributeName("hx-target")]
    public string? HxTarget { get; set; }

    /// <summary>
    /// How the response will be swapped relative to the target (e.g., innerHTML, outerHTML, beforeend).
    /// </summary>
    [HtmlAttributeName("hx-swap")]
    public string? HxSwap { get; set; }

    /// <summary>
    /// The event that triggers the request (e.g., click, change, revealed).
    /// </summary>
    [HtmlAttributeName("hx-trigger")]
    public string? HxTrigger { get; set; }

    /// <summary>
    /// CSS selector of the element to show as a loading indicator during the request.
    /// </summary>
    [HtmlAttributeName("hx-indicator")]
    public string? HxIndicator { get; set; }

    /// <summary>
    /// Shows a confirm dialog with this message before issuing the request.
    /// </summary>
    [HtmlAttributeName("hx-confirm")]
    public string? HxConfirm { get; set; }

    /// <summary>
    /// Pushes the request URL into the browser history. Can be "true" or a specific URL.
    /// </summary>
    [HtmlAttributeName("hx-push-url")]
    public string? HxPushUrl { get; set; }

    /// <summary>
    /// Enables progressive enhancement — links and forms use htmx but fall back to normal behavior.
    /// </summary>
    [HtmlAttributeName("hx-boost")]
    public string? HxBoost { get; set; }

    /// <summary>
    /// JSON-formatted additional values to include with the request.
    /// </summary>
    [HtmlAttributeName("hx-vals")]
    public string? HxVals { get; set; }

    /// <summary>
    /// JSON-formatted additional headers to include with the request.
    /// </summary>
    [HtmlAttributeName("hx-headers")]
    public string? HxHeaders { get; set; }

    /// <summary>
    /// CSS selector for the element to disable during the request.
    /// </summary>
    [HtmlAttributeName("hx-disabled-elt")]
    public string? HxDisabledElt { get; set; }

    /// <summary>
    /// Encoding type for the request (e.g., multipart/form-data).
    /// </summary>
    [HtmlAttributeName("hx-encoding")]
    public string? HxEncoding { get; set; }

    /// <summary>
    /// htmx extensions to enable on this element (e.g., "json-enc").
    /// </summary>
    [HtmlAttributeName("hx-ext")]
    public string? HxExt { get; set; }

    /// <summary>
    /// CSS selector of additional inputs to include with the request.
    /// </summary>
    [HtmlAttributeName("hx-include")]
    public string? HxInclude { get; set; }

    /// <summary>
    /// Filters the parameters to send with the request (* for all, none, or a CSS selector).
    /// </summary>
    [HtmlAttributeName("hx-params")]
    public string? HxParams { get; set; }

    /// <summary>
    /// CSS selector to pick content from the response for swapping.
    /// </summary>
    [HtmlAttributeName("hx-select")]
    public string? HxSelect { get; set; }

    /// <summary>
    /// CSS selector to pick content from the response for out-of-band swapping.
    /// </summary>
    [HtmlAttributeName("hx-select-oob")]
    public string? HxSelectOob { get; set; }

    /// <summary>
    /// Marks this element for out-of-band swapping from a response.
    /// </summary>
    [HtmlAttributeName("hx-swap-oob")]
    public string? HxSwapOob { get; set; }

    /// <summary>
    /// Coordinates request synchronization with other elements.
    /// </summary>
    [HtmlAttributeName("hx-sync")]
    public string? HxSync { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a new instance with URL generation support.
    /// </summary>
    protected RazorHXTagHelperBase(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory = urlHelperFactory;
    }

    /// <summary>
    /// Creates a new instance without URL generation support.
    /// Use this constructor for components that don't need route-based URL generation.
    /// </summary>
    protected RazorHXTagHelperBase()
    {
        _urlHelperFactory = null;
    }

    // ──────────────────────────────────────────────
    //  BEM class name helpers
    // ──────────────────────────────────────────────

    /// <summary>
    /// Gets the full BEM block class (e.g., "rhx-button").
    /// </summary>
    protected string GetBlockClass() => $"{CssPrefix}{BlockName}";

    /// <summary>
    /// Gets a BEM modifier class (e.g., "rhx-button--primary").
    /// </summary>
    protected string GetModifierClass(string modifier) => $"{CssPrefix}{BlockName}--{modifier}";

    /// <summary>
    /// Gets a BEM element class (e.g., "rhx-button__icon").
    /// </summary>
    protected string GetElementClass(string element) => $"{CssPrefix}{BlockName}__{element}";

    /// <summary>
    /// Creates a new <see cref="CssClassBuilder"/> pre-initialized with this component's block class.
    /// </summary>
    protected CssClassBuilder CreateCssBuilder() => new CssClassBuilder(GetBlockClass());

    // ──────────────────────────────────────────────
    //  CSS class helpers
    // ──────────────────────────────────────────────

    /// <summary>
    /// Builds a space-separated CSS class string from the provided classes,
    /// filtering out nulls and empty strings.
    /// </summary>
    /// <param name="classes">CSS class names to join (null and empty values are ignored).</param>
    /// <returns>A single space-separated CSS class string.</returns>
    protected static string BuildCssClass(params string?[] classes)
    {
        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }

    /// <summary>
    /// Returns the CSS class name if the condition is true; otherwise returns null.
    /// Useful for conditional class inclusion in <see cref="BuildCssClass"/>.
    /// </summary>
    /// <param name="className">The CSS class to conditionally include.</param>
    /// <param name="condition">Whether to include the class.</param>
    /// <returns>The class name or null.</returns>
    protected static string? ConditionalCssClass(string className, bool condition)
    {
        return condition ? className : null;
    }

    /// <summary>
    /// Returns a BEM variant class "rhx-{block}--{variant}" if variant is non-null/empty.
    /// </summary>
    /// <param name="block">The BEM block name (e.g., "button").</param>
    /// <param name="variant">The variant name (e.g., "brand", "danger").</param>
    /// <returns>The full variant class or null.</returns>
    protected static string? VariantCssClass(string block, string? variant)
    {
        return string.IsNullOrWhiteSpace(variant)
            ? null
            : $"{CssPrefix}{block}--{variant}";
    }

    /// <summary>
    /// Returns a BEM size class "rhx-{block}--{size}" if size is non-null/empty.
    /// </summary>
    /// <param name="block">The BEM block name (e.g., "button").</param>
    /// <param name="size">The size name (e.g., "small", "large").</param>
    /// <returns>The full size class or null.</returns>
    protected static string? SizeCssClass(string block, string? size)
    {
        return string.IsNullOrWhiteSpace(size)
            ? null
            : $"{CssPrefix}{block}--{size}";
    }

    // ──────────────────────────────────────────────
    //  Attribute rendering
    // ──────────────────────────────────────────────

    /// <summary>
    /// Applies the computed CSS classes and common HTML attributes (id, hidden) to the output.
    /// </summary>
    /// <param name="output">The tag helper output to modify.</param>
    /// <param name="cssBuilder">The CSS class builder containing the component's classes.</param>
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

    /// <summary>
    /// Writes all non-null htmx attributes to the tag output.
    /// Handles route URL generation: when a verb attribute (hx-get, hx-post, etc.) is set
    /// to empty string and hx-page/hx-controller/hx-action are specified, the URL is
    /// generated from ASP.NET Core routing.
    /// </summary>
    /// <param name="output">The tag helper output to write htmx attributes to.</param>
    protected void RenderHtmxAttributes(TagHelperOutput output)
    {
        // Resolve URLs for verb attributes (route generation when value is empty)
        WriteVerbAttribute(output, "hx-get", HxGet);
        WriteVerbAttribute(output, "hx-post", HxPost);
        WriteVerbAttribute(output, "hx-put", HxPut);
        WriteVerbAttribute(output, "hx-patch", HxPatch);
        WriteVerbAttribute(output, "hx-delete", HxDelete);

        // Behavior attributes
        WriteAttribute(output, "hx-target", HxTarget);
        WriteAttribute(output, "hx-swap", HxSwap);
        WriteAttribute(output, "hx-trigger", HxTrigger);
        WriteAttribute(output, "hx-indicator", HxIndicator);
        WriteAttribute(output, "hx-confirm", HxConfirm);
        WriteAttribute(output, "hx-push-url", HxPushUrl);
        WriteAttribute(output, "hx-boost", HxBoost);
        WriteAttribute(output, "hx-vals", HxVals);
        WriteAttribute(output, "hx-headers", HxHeaders);
        WriteAttribute(output, "hx-disabled-elt", HxDisabledElt);
        WriteAttribute(output, "hx-encoding", HxEncoding);
        WriteAttribute(output, "hx-ext", HxExt);
        WriteAttribute(output, "hx-include", HxInclude);
        WriteAttribute(output, "hx-params", HxParams);
        WriteAttribute(output, "hx-select", HxSelect);
        WriteAttribute(output, "hx-select-oob", HxSelectOob);
        WriteAttribute(output, "hx-swap-oob", HxSwapOob);
        WriteAttribute(output, "hx-sync", HxSync);
    }

    /// <summary>
    /// Generates a URL from the configured route properties (hx-page, hx-controller, hx-action, hx-route-*).
    /// Returns null if no route properties are set.
    /// </summary>
    /// <returns>The generated URL, or null if route generation is not configured.</returns>
    protected internal string? GenerateRouteUrl()
    {
        if (_urlHelperFactory is null || ViewContext is null)
            return null;

        var hasPage = !string.IsNullOrWhiteSpace(HxPage);
        var hasAction = !string.IsNullOrWhiteSpace(HxAction) || !string.IsNullOrWhiteSpace(HxController);

        if (!hasPage && !hasAction)
            return null;

        var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
        var routeValues = HxRouteValues.Count > 0
            ? new RouteValueDictionary(HxRouteValues.ToDictionary(k => k.Key, v => (object?)v.Value))
            : null;

        if (hasPage)
        {
            if (routeValues is not null && !string.IsNullOrWhiteSpace(HxHandler))
            {
                routeValues["handler"] = HxHandler;
            }
            else if (!string.IsNullOrWhiteSpace(HxHandler))
            {
                routeValues = new RouteValueDictionary { ["handler"] = HxHandler };
            }

            return urlHelper.Page(HxPage, routeValues);
        }

        return urlHelper.Action(HxAction, HxController, routeValues);
    }

    // ──────────────────────────────────────────────
    //  Private helpers
    // ──────────────────────────────────────────────

    /// <summary>
    /// Writes an htmx verb attribute. If the value is exactly empty string (not null),
    /// attempts to generate a route URL. If the value is non-empty, writes it directly.
    /// </summary>
    private void WriteVerbAttribute(TagHelperOutput output, string attrName, string? value)
    {
        if (value is null)
            return;

        if (value.Length == 0)
        {
            // Empty string signals "generate URL from route properties"
            var generatedUrl = GenerateRouteUrl();
            if (!string.IsNullOrWhiteSpace(generatedUrl))
            {
                output.Attributes.SetAttribute(attrName, generatedUrl);
            }
        }
        else
        {
            output.Attributes.SetAttribute(attrName, value);
        }
    }

    private static void WriteAttribute(TagHelperOutput output, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            output.Attributes.SetAttribute(name, value);
        }
    }
}
