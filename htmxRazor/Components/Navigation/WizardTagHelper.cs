using System.Text.Encodings.Web;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Multi-step wizard with a visual stepper indicator, auto-generated navigation
/// buttons, and server-side step state tracking via htmx requests.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-wizard rhx-current-step="2" page="/Checkout"&gt;
///     &lt;rhx-wizard-step rhx-title="Account" rhx-status="complete"&gt;...&lt;/rhx-wizard-step&gt;
///     &lt;rhx-wizard-step rhx-title="Shipping" rhx-status="current"&gt;...&lt;/rhx-wizard-step&gt;
///     &lt;rhx-wizard-step rhx-title="Payment"&gt;...&lt;/rhx-wizard-step&gt;
/// &lt;/rhx-wizard&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-wizard")]
public class WizardTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "wizard";

    // ──────────────────────────────────────────────
    //  Step properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The current step number (1-based). Default: 1.
    /// </summary>
    [HtmlAttributeName("rhx-current-step")]
    public int CurrentStep { get; set; } = 1;

    /// <summary>
    /// The total number of steps. When 0, auto-counted from child steps.
    /// </summary>
    [HtmlAttributeName("rhx-total-steps")]
    public int TotalSteps { get; set; }

    /// <summary>
    /// The stepper layout. Default: "horizontal".
    /// Accepted values: <c>horizontal</c>, <c>vertical</c>.
    /// </summary>
    [HtmlAttributeName("rhx-layout")]
    public string Layout { get; set; } = "horizontal";

    /// <summary>
    /// Whether to show prev/next navigation buttons. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-show-nav")]
    public bool ShowNav { get; set; } = true;

    /// <summary>
    /// Whether steps must be completed in order. Default: true.
    /// When false, users can click completed step indicators to navigate.
    /// </summary>
    [HtmlAttributeName("rhx-linear")]
    public bool Linear { get; set; } = true;

    // ──────────────────────────────────────────────
    //  Route properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page path for wizard navigation URLs.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler for the Previous button. Default: "WizardPrev".
    /// </summary>
    [HtmlAttributeName("page-handler-prev")]
    public string PrevHandler { get; set; } = "WizardPrev";

    /// <summary>
    /// The page handler for the Next button. Default: "WizardNext".
    /// </summary>
    [HtmlAttributeName("page-handler-next")]
    public string NextHandler { get; set; } = "WizardNext";

    /// <summary>
    /// The page handler for clicking a step indicator. Default: "WizardStep".
    /// </summary>
    [HtmlAttributeName("page-handler-step")]
    public string StepHandler { get; set; } = "WizardStep";

    /// <summary>
    /// Route parameter values for URL generation. Bound from route-* attributes.
    /// </summary>
    [HtmlAttributeName("route-", DictionaryAttributePrefix = "route-")]
    public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a new <see cref="WizardTagHelper"/> instance.
    /// </summary>
    public WizardTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Register step list so children can populate it.
        // Reuse existing list if already present (e.g., in tests).
        WizardStepList steps;
        if (context.Items.TryGetValue(typeof(WizardStepList), out var existing) && existing is WizardStepList existingSteps)
        {
            steps = existingSteps;
        }
        else
        {
            steps = new WizardStepList();
            context.Items[typeof(WizardStepList)] = steps;
        }

        // Process children to collect step metadata
        await output.GetChildContentAsync();

        var totalSteps = TotalSteps > 0 ? TotalSteps : steps.Count;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .Add(GetModifierClass(Layout));

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", "group");
        output.Attributes.SetAttribute("data-rhx-wizard", "");

        RenderHtmxAttributes(output);

        // ── Stepper indicator ──
        output.Content.AppendHtml("<nav class=\"rhx-wizard__stepper\" aria-label=\"Progress\">");
        output.Content.AppendHtml("<ol class=\"rhx-wizard__steps\" role=\"list\">");

        for (var i = 0; i < steps.Count; i++)
        {
            var step = steps[i];
            var stepNum = i + 1;
            var isCurrent = stepNum == CurrentStep;
            var statusClass = step.Status switch
            {
                "complete" => "rhx-wizard__step--complete",
                "current" => "rhx-wizard__step--current",
                "error" => "rhx-wizard__step--error",
                _ => "rhx-wizard__step--incomplete"
            };
            if (isCurrent)
                statusClass = "rhx-wizard__step--current";

            var ariaCurrent = isCurrent ? " aria-current=\"step\"" : "";

            output.Content.AppendHtml($"<li class=\"rhx-wizard__step {statusClass}\" role=\"listitem\"{ariaCurrent}>");

            // Step indicator (number or checkmark)
            if (step.Status == "complete" && !isCurrent)
            {
                output.Content.AppendHtml("<span class=\"rhx-wizard__step-indicator\" aria-hidden=\"true\">");
                output.Content.AppendHtml("<svg width=\"16\" height=\"16\" viewBox=\"0 0 16 16\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M13.5 4.5L6 12L2.5 8.5\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/></svg>");
                output.Content.AppendHtml("</span>");
            }
            else
            {
                // Render step number; for non-linear, make clickable if complete
                var clickable = !Linear && step.Status == "complete" && !isCurrent;
                if (clickable)
                {
                    var stepUrl = GenerateStepUrl(stepNum);
                    output.Content.AppendHtml(
                        $"<button class=\"rhx-wizard__step-indicator\" type=\"button\" hx-get=\"{stepUrl}\" hx-target=\"closest .rhx-wizard\" hx-swap=\"outerHTML\">");
                    output.Content.AppendHtml($"{stepNum}");
                    output.Content.AppendHtml("</button>");
                }
                else
                {
                    output.Content.AppendHtml($"<span class=\"rhx-wizard__step-indicator\" aria-hidden=\"true\">{stepNum}</span>");
                }
            }

            // Step label
            output.Content.AppendHtml("<span class=\"rhx-wizard__step-label\">");
            output.Content.AppendHtml($"<span class=\"rhx-wizard__step-title\">{HtmlEncoder.Default.Encode(step.Title)}</span>");
            if (!string.IsNullOrWhiteSpace(step.Description))
            {
                output.Content.AppendHtml($"<span class=\"rhx-wizard__step-description\">{HtmlEncoder.Default.Encode(step.Description)}</span>");
            }
            output.Content.AppendHtml("</span>");

            output.Content.AppendHtml("</li>");
        }

        output.Content.AppendHtml("</ol>");
        output.Content.AppendHtml("</nav>");

        // ── Step content panel ──
        var currentStepIndex = CurrentStep - 1;
        var currentStepTitle = currentStepIndex >= 0 && currentStepIndex < steps.Count
            ? steps[currentStepIndex].Title
            : "";

        output.Content.AppendHtml(
            $"<div class=\"rhx-wizard__panel\" role=\"region\" aria-label=\"Step {CurrentStep}: {HtmlEncoder.Default.Encode(currentStepTitle)}\">");

        if (currentStepIndex >= 0 && currentStepIndex < steps.Count && steps[currentStepIndex].Content is not null)
        {
            output.Content.AppendHtml(steps[currentStepIndex].Content!);
        }

        output.Content.AppendHtml("</div>");

        // ── Navigation buttons ──
        if (ShowNav)
        {
            output.Content.AppendHtml("<div class=\"rhx-wizard__nav\" role=\"navigation\" aria-label=\"Wizard navigation\">");

            // Previous button (hidden on first step)
            if (CurrentStep > 1)
            {
                var prevUrl = GenerateNavUrl(PrevHandler, CurrentStep - 1);
                output.Content.AppendHtml(
                    $"<button class=\"rhx-wizard__prev rhx-button rhx-button--neutral rhx-button--outlined\" type=\"button\" hx-get=\"{prevUrl}\" hx-target=\"closest .rhx-wizard\" hx-swap=\"outerHTML\">Previous</button>");
            }
            else
            {
                // Empty spacer to maintain layout
                output.Content.AppendHtml("<span></span>");
            }

            // Next / Submit button
            var isLast = CurrentStep >= totalSteps;
            var nextLabel = isLast ? "Submit" : "Next";
            var nextUrl = GenerateNavUrl(NextHandler, CurrentStep + 1);
            output.Content.AppendHtml(
                $"<button class=\"rhx-wizard__next rhx-button rhx-button--brand rhx-button--filled\" type=\"button\" hx-post=\"{nextUrl}\" hx-target=\"closest .rhx-wizard\" hx-swap=\"outerHTML\">{nextLabel}</button>");

            output.Content.AppendHtml("</div>");
        }
    }

    private string GenerateNavUrl(string handler, int step)
    {
        HxPage = Page;
        HxHandler = handler;
        HxRouteValues.Clear();
        foreach (var kv in RouteValues)
            HxRouteValues[kv.Key] = kv.Value;
        HxRouteValues["step"] = step.ToString();
        return GenerateRouteUrl() ?? "#";
    }

    private string GenerateStepUrl(int step)
    {
        return GenerateNavUrl(StepHandler, step);
    }
}
