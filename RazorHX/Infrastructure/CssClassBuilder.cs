namespace RazorHX.Infrastructure;

/// <summary>
/// Fluent builder for constructing CSS class strings.
/// </summary>
public sealed class CssClassBuilder
{
    private readonly List<string> _classes = [];

    public CssClassBuilder() { }

    public CssClassBuilder(string initialClass)
    {
        if (!string.IsNullOrWhiteSpace(initialClass))
        {
            _classes.Add(initialClass);
        }
    }

    /// <summary>
    /// Adds a CSS class unconditionally.
    /// </summary>
    public CssClassBuilder Add(string className)
    {
        if (!string.IsNullOrWhiteSpace(className))
        {
            _classes.Add(className);
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class only when the condition is true.
    /// </summary>
    public CssClassBuilder AddIf(string className, bool condition)
    {
        if (condition && !string.IsNullOrWhiteSpace(className))
        {
            _classes.Add(className);
        }
        return this;
    }

    /// <summary>
    /// Adds one of two CSS classes depending on the condition.
    /// </summary>
    public CssClassBuilder AddChoice(bool condition, string trueClass, string falseClass)
    {
        _classes.Add(condition ? trueClass : falseClass);
        return this;
    }

    /// <summary>
    /// Adds a CSS class derived from a nullable enum value.
    /// </summary>
    public CssClassBuilder AddEnum<T>(string prefix, T? value) where T : struct, Enum
    {
        if (value.HasValue)
        {
            _classes.Add($"{prefix}{value.Value.ToString().ToLowerInvariant()}");
        }
        return this;
    }

    /// <summary>
    /// Builds the final space-separated CSS class string.
    /// </summary>
    public string Build() => string.Join(" ", _classes);

    /// <summary>
    /// Returns true if no classes have been added.
    /// </summary>
    public bool IsEmpty => _classes.Count == 0;

    public override string ToString() => Build();
}
