using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Metadata about a wizard step registered as a child of <see cref="WizardTagHelper"/>.
/// The step's child content is only rendered when this step is the current step.
/// </summary>
[HtmlTargetElement("rhx-wizard-step", ParentTag = "rhx-wizard")]
public class WizardStepTagHelper : TagHelper
{
    /// <summary>
    /// The display title for the step (shown in the stepper indicator).
    /// </summary>
    [HtmlAttributeName("rhx-title")]
    public string Title { get; set; } = "";

    /// <summary>
    /// Optional description text shown below the step title.
    /// </summary>
    [HtmlAttributeName("rhx-description")]
    public string? Description { get; set; }

    /// <summary>
    /// Optional icon name for the step indicator.
    /// </summary>
    [HtmlAttributeName("rhx-icon")]
    public string? Icon { get; set; }

    /// <summary>
    /// The step status. Default: "incomplete".
    /// Accepted values: <c>incomplete</c>, <c>current</c>, <c>complete</c>, <c>error</c>.
    /// </summary>
    [HtmlAttributeName("rhx-status")]
    public string Status { get; set; } = "incomplete";

    /// <summary>
    /// Whether this step is optional and can be skipped.
    /// </summary>
    [HtmlAttributeName("rhx-optional")]
    public bool Optional { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Get the step list from the parent context
        if (context.Items.TryGetValue(typeof(WizardStepList), out var obj) && obj is WizardStepList steps)
        {
            var childContent = await output.GetChildContentAsync();
            steps.Add(new WizardStepData
            {
                Title = Title,
                Description = Description,
                Icon = Icon,
                Status = Status,
                Optional = Optional,
                Content = childContent
            });
        }

        // Suppress output — the parent wizard renders step content
        output.SuppressOutput();
    }
}

/// <summary>
/// Data holder for wizard step metadata and content.
/// </summary>
public sealed class WizardStepData
{
    public string Title { get; init; } = "";
    public string? Description { get; init; }
    public string? Icon { get; init; }
    public string Status { get; init; } = "incomplete";
    public bool Optional { get; init; }
    public TagHelperContent? Content { get; init; }
}

/// <summary>
/// List type for collecting step data from child tag helpers.
/// </summary>
public sealed class WizardStepList : List<WizardStepData>;
