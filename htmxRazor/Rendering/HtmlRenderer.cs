using System.Text;
using Microsoft.AspNetCore.Html;

namespace htmxRazor.Rendering;

/// <summary>
/// Structured HTML output builder for constructing component markup.
/// </summary>
public sealed class HtmlRenderer
{
    private readonly StringBuilder _sb = new();
    private int _indent;

    /// <summary>
    /// Opens an HTML tag with optional attributes.
    /// </summary>
    public HtmlRenderer OpenTag(string tag, IDictionary<string, string>? attributes = null)
    {
        AppendIndent();
        _sb.Append($"<{tag}");
        if (attributes != null)
        {
            foreach (var (key, value) in attributes)
            {
                _sb.Append($" {key}=\"{Encode(value)}\"");
            }
        }
        _sb.AppendLine(">");
        _indent++;
        return this;
    }

    /// <summary>
    /// Opens a self-closing/void HTML tag.
    /// </summary>
    public HtmlRenderer VoidTag(string tag, IDictionary<string, string>? attributes = null)
    {
        AppendIndent();
        _sb.Append($"<{tag}");
        if (attributes != null)
        {
            foreach (var (key, value) in attributes)
            {
                _sb.Append($" {key}=\"{Encode(value)}\"");
            }
        }
        _sb.AppendLine(" />");
        return this;
    }

    /// <summary>
    /// Closes an HTML tag.
    /// </summary>
    public HtmlRenderer CloseTag(string tag)
    {
        _indent = Math.Max(0, _indent - 1);
        AppendIndent();
        _sb.AppendLine($"</{tag}>");
        return this;
    }

    /// <summary>
    /// Appends raw text content.
    /// </summary>
    public HtmlRenderer Text(string text)
    {
        AppendIndent();
        _sb.AppendLine(Encode(text));
        return this;
    }

    /// <summary>
    /// Appends pre-encoded HTML content.
    /// </summary>
    public HtmlRenderer RawHtml(string html)
    {
        AppendIndent();
        _sb.AppendLine(html);
        return this;
    }

    /// <summary>
    /// Builds the final HTML string.
    /// </summary>
    public IHtmlContent Build() => new HtmlString(_sb.ToString());

    public override string ToString() => _sb.ToString();

    private void AppendIndent()
    {
        for (var i = 0; i < _indent; i++)
        {
            _sb.Append("  ");
        }
    }

    private static string Encode(string value)
        => System.Net.WebUtility.HtmlEncode(value);
}
