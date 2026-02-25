using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Forms;

/// <summary>
/// Abstract base class for all htmxRazor form control tag helpers.
/// Provides shared logic for ASP.NET Core model binding, validation,
/// label/hint/error rendering, and htmx attribute forwarding to inner elements.
/// </summary>
public abstract class FormControlTagHelperBase : htmxRazorTagHelperBase
{
    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    protected FormControlTagHelperBase(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Model binding
    // ──────────────────────────────────────────────

    /// <summary>
    /// ASP.NET Core model expression for two-way binding.
    /// When set, auto-generates name, id, value, type, required, constraints, label, hint,
    /// and validation attributes from the model metadata.
    /// </summary>
    [HtmlAttributeName("rhx-for")]
    public ModelExpression? For { get; set; }

    // ──────────────────────────────────────────────
    //  Common form properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The form field name. Falls back to the expression path from <see cref="For"/>.
    /// </summary>
    [HtmlAttributeName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The current value. Falls back to the model value from <see cref="For"/>.
    /// </summary>
    [HtmlAttributeName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// The label text. Falls back to DisplayName from model metadata.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Hint text displayed below the control. Falls back to Description from model metadata.
    /// </summary>
    [HtmlAttributeName("rhx-hint")]
    public string? Hint { get; set; }

    /// <summary>
    /// The size of the form control. Options: small, medium, large. Default: medium.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string Size { get; set; } = "medium";

    /// <summary>
    /// Whether the control is disabled.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Whether the control is read-only.
    /// </summary>
    [HtmlAttributeName("rhx-readonly")]
    public bool Readonly { get; set; }

    /// <summary>
    /// Whether the field is required. Auto-set from [Required] when <see cref="For"/> is bound.
    /// </summary>
    [HtmlAttributeName("rhx-required")]
    public bool Required { get; set; }

    /// <summary>
    /// Accessible label for the native input element.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    // ──────────────────────────────────────────────
    //  Resolution methods
    // ──────────────────────────────────────────────

    /// <summary>Resolves the form field name from explicit property or model expression.</summary>
    protected string ResolveName() => Name ?? For?.Name ?? "";

    /// <summary>Resolves the HTML id from explicit Id or the resolved name (dots/brackets to underscores).</summary>
    protected string ResolveId()
    {
        if (!string.IsNullOrEmpty(Id)) return Id;
        return ResolveName().Replace('.', '_').Replace('[', '_').Replace(']', '_');
    }

    /// <summary>Resolves the current value from explicit property or model.</summary>
    protected string? ResolveValue()
    {
        if (Value != null) return Value;
        return For?.Model?.ToString();
    }

    /// <summary>Resolves the label text from explicit property, DisplayName, or property name.</summary>
    protected string? ResolveLabelText()
    {
        if (Label != null) return Label;
        if (For != null)
        {
            var display = For.Metadata.DisplayName;
            if (!string.IsNullOrEmpty(display)) return display;
            return For.Name;
        }
        return null;
    }

    /// <summary>Resolves the hint text from explicit property or model Description.</summary>
    protected string? ResolveHint()
    {
        if (Hint != null) return Hint;
        return For?.Metadata.Description;
    }

    /// <summary>Resolves whether the field is required from explicit property or model metadata.</summary>
    protected bool ResolveRequired()
    {
        if (Required) return true;
        return For?.Metadata.IsRequired == true;
    }

    /// <summary>Reads the first validation error from ModelState for this field, or null.</summary>
    protected string? ResolveErrorMessage()
    {
        var name = ResolveName();
        if (string.IsNullOrEmpty(name)) return null;

        if (ViewContext.ModelState.TryGetValue(name, out var entry) && entry.Errors.Count > 0)
        {
            return entry.Errors[0].ErrorMessage;
        }
        return null;
    }

    /// <summary>Returns true if this field has a validation error in ModelState.</summary>
    protected bool HasError() => ResolveErrorMessage() != null;

    // ──────────────────────────────────────────────
    //  Constraint extraction from model metadata
    // ──────────────────────────────────────────────

    /// <summary>Extracts minlength from [StringLength] or [MinLength] validator metadata.</summary>
    protected int? ExtractMinlength()
    {
        if (For?.Metadata.ValidatorMetadata == null) return null;
        foreach (var v in For.Metadata.ValidatorMetadata)
        {
            if (v is StringLengthAttribute sl && sl.MinimumLength > 0) return sl.MinimumLength;
            if (v is MinLengthAttribute ml) return ml.Length;
        }
        return null;
    }

    /// <summary>Extracts maxlength from [StringLength] or [MaxLength] validator metadata.</summary>
    protected int? ExtractMaxlength()
    {
        if (For?.Metadata.ValidatorMetadata == null) return null;
        foreach (var v in For.Metadata.ValidatorMetadata)
        {
            if (v is StringLengthAttribute sl) return sl.MaximumLength;
            if (v is MaxLengthAttribute ml) return ml.Length;
        }
        return null;
    }

    /// <summary>Extracts pattern from [RegularExpression] validator metadata.</summary>
    protected string? ExtractPattern()
    {
        if (For?.Metadata.ValidatorMetadata == null) return null;
        foreach (var v in For.Metadata.ValidatorMetadata)
        {
            if (v is RegularExpressionAttribute re) return re.Pattern;
        }
        return null;
    }

    // ──────────────────────────────────────────────
    //  HTML helpers
    // ──────────────────────────────────────────────

    /// <summary>HTML-encodes a string value for safe embedding in attributes and content.</summary>
    protected static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";

    /// <summary>Builds the label HTML element.</summary>
    protected string BuildLabelHtml(string inputId)
    {
        var label = ResolveLabelText();
        if (string.IsNullOrEmpty(label)) return "";
        return $"<label class=\"{GetElementClass("label")}\" for=\"{Enc(inputId)}\">{Enc(label)}</label>";
    }

    /// <summary>Builds the hint span HTML element.</summary>
    protected string BuildHintHtml(string hintId)
    {
        var hint = ResolveHint();
        if (string.IsNullOrEmpty(hint)) return "";
        return $"<span class=\"{GetElementClass("hint")}\" id=\"{Enc(hintId)}\">{Enc(hint)}</span>";
    }

    /// <summary>Builds the error span HTML element (hidden when no error).</summary>
    protected string BuildErrorHtml(string errorId)
    {
        var error = ResolveErrorMessage();
        var cls = GetElementClass("error");
        if (string.IsNullOrEmpty(error))
        {
            return $"<span class=\"{cls}\" id=\"{Enc(errorId)}\" aria-live=\"polite\" hidden></span>";
        }
        return $"<span class=\"{cls}\" id=\"{Enc(errorId)}\" aria-live=\"polite\">{Enc(error)}</span>";
    }

    /// <summary>Builds the aria-describedby value from hint and error element ids.</summary>
    protected string? BuildAriaDescribedBy(string hintId, string errorId)
    {
        var parts = new List<string>();
        if (!string.IsNullOrEmpty(ResolveHint())) parts.Add(hintId);
        if (HasError()) parts.Add(errorId);
        return parts.Count > 0 ? string.Join(" ", parts) : null;
    }

    // ──────────────────────────────────────────────
    //  htmx attribute string builder
    // ──────────────────────────────────────────────

    /// <summary>
    /// Builds all htmx attributes as a raw HTML attribute string for embedding
    /// in inner element markup. This is used because htmx attributes must go on
    /// the native input element, not the wrapper div.
    /// </summary>
    protected string BuildHtmxAttributeString()
    {
        var sb = new StringBuilder();

        void VerbAttr(string name, string? value)
        {
            if (value == null) return;
            if (value.Length == 0)
            {
                var url = GenerateRouteUrl();
                if (!string.IsNullOrWhiteSpace(url))
                    sb.Append($" {name}=\"{Enc(url)}\"");
            }
            else
            {
                sb.Append($" {name}=\"{Enc(value)}\"");
            }
        }

        void Attr(string name, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                sb.Append($" {name}=\"{Enc(value)}\"");
        }

        VerbAttr("hx-get", HxGet);
        VerbAttr("hx-post", HxPost);
        VerbAttr("hx-put", HxPut);
        VerbAttr("hx-patch", HxPatch);
        VerbAttr("hx-delete", HxDelete);
        Attr("hx-target", HxTarget);
        Attr("hx-swap", HxSwap);
        Attr("hx-trigger", HxTrigger);
        Attr("hx-indicator", HxIndicator);
        Attr("hx-confirm", HxConfirm);
        Attr("hx-push-url", HxPushUrl);
        Attr("hx-boost", HxBoost);
        Attr("hx-vals", HxVals);
        Attr("hx-headers", HxHeaders);
        Attr("hx-disabled-elt", HxDisabledElt);
        Attr("hx-encoding", HxEncoding);
        Attr("hx-ext", HxExt);
        Attr("hx-include", HxInclude);
        Attr("hx-params", HxParams);
        Attr("hx-select", HxSelect);
        Attr("hx-select-oob", HxSelectOob);
        Attr("hx-swap-oob", HxSwapOob);
        Attr("hx-sync", HxSync);

        return sb.ToString();
    }

    // ──────────────────────────────────────────────
    //  Validation data attribute string builder
    // ──────────────────────────────────────────────

    /// <summary>
    /// Builds data-val-* attributes for unobtrusive client-side validation
    /// by inspecting the model metadata's ValidatorMetadata collection.
    /// </summary>
    protected string BuildValidationAttributeString()
    {
        if (For?.Metadata.ValidatorMetadata == null || For.Metadata.ValidatorMetadata.Count == 0)
            return "";

        var sb = new StringBuilder();
        sb.Append(" data-val=\"true\"");

        var labelText = ResolveLabelText() ?? ResolveName();

        foreach (var validator in For.Metadata.ValidatorMetadata)
        {
            switch (validator)
            {
                case RequiredAttribute req:
                    var reqMsg = !string.IsNullOrEmpty(req.ErrorMessage)
                        ? req.ErrorMessage
                        : $"The {labelText} field is required.";
                    sb.Append($" data-val-required=\"{Enc(reqMsg)}\"");
                    break;

                case StringLengthAttribute sl:
                    var slMsg = !string.IsNullOrEmpty(sl.ErrorMessage)
                        ? sl.ErrorMessage
                        : $"The field {labelText} must be a string with a maximum length of {sl.MaximumLength}.";
                    sb.Append($" data-val-length=\"{Enc(slMsg)}\"");
                    sb.Append($" data-val-length-max=\"{sl.MaximumLength}\"");
                    if (sl.MinimumLength > 0)
                        sb.Append($" data-val-length-min=\"{sl.MinimumLength}\"");
                    break;

                case MaxLengthAttribute maxl:
                    var maxlMsg = !string.IsNullOrEmpty(maxl.ErrorMessage)
                        ? maxl.ErrorMessage
                        : $"The field {labelText} must be a string or array type with a maximum length of '{maxl.Length}'.";
                    sb.Append($" data-val-maxlength=\"{Enc(maxlMsg)}\"");
                    sb.Append($" data-val-maxlength-max=\"{maxl.Length}\"");
                    break;

                case MinLengthAttribute minl:
                    var minlMsg = !string.IsNullOrEmpty(minl.ErrorMessage)
                        ? minl.ErrorMessage
                        : $"The field {labelText} must be a string or array type with a minimum length of '{minl.Length}'.";
                    sb.Append($" data-val-minlength=\"{Enc(minlMsg)}\"");
                    sb.Append($" data-val-minlength-min=\"{minl.Length}\"");
                    break;

                case RegularExpressionAttribute regex:
                    var regexMsg = !string.IsNullOrEmpty(regex.ErrorMessage)
                        ? regex.ErrorMessage
                        : $"The field {labelText} must match the regular expression '{regex.Pattern}'.";
                    sb.Append($" data-val-regex=\"{Enc(regexMsg)}\"");
                    sb.Append($" data-val-regex-pattern=\"{Enc(regex.Pattern)}\"");
                    break;

                case RangeAttribute range:
                    var rangeMsg = !string.IsNullOrEmpty(range.ErrorMessage)
                        ? range.ErrorMessage
                        : $"The field {labelText} must be between {range.Minimum} and {range.Maximum}.";
                    sb.Append($" data-val-range=\"{Enc(rangeMsg)}\"");
                    sb.Append($" data-val-range-min=\"{range.Minimum}\"");
                    sb.Append($" data-val-range-max=\"{range.Maximum}\"");
                    break;

                case EmailAddressAttribute:
                    sb.Append($" data-val-email=\"The {Enc(labelText)} field is not a valid e-mail address.\"");
                    break;

                case UrlAttribute:
                    sb.Append($" data-val-url=\"The {Enc(labelText)} field is not a valid fully-qualified http, https, or ftp URL.\"");
                    break;

                case PhoneAttribute:
                    sb.Append($" data-val-phone=\"The {Enc(labelText)} field is not a valid phone number.\"");
                    break;
            }
        }

        return sb.ToString();
    }

    // ──────────────────────────────────────────────
    //  Wrapper attribute helper
    // ──────────────────────────────────────────────

    /// <summary>
    /// Applies CSS classes and hidden attribute to the wrapper div.
    /// Unlike <see cref="htmxRazorTagHelperBase.ApplyBaseAttributes"/>, this does NOT set the id
    /// on the wrapper — form controls place id on the native input element.
    /// </summary>
    protected void ApplyWrapperAttributes(TagHelperOutput output, CssClassBuilder css)
    {
        if (!string.IsNullOrWhiteSpace(CssClass))
            css.Add(CssClass);

        output.Attributes.SetAttribute("class", css.Build());

        if (Hidden)
            output.Attributes.SetAttribute("hidden", "hidden");
    }
}
