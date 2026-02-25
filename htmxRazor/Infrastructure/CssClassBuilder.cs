namespace htmxRazor.Infrastructure;

/// <summary>
/// Fluent builder for constructing CSS class strings.
/// Supports conditional inclusion, enum-based modifiers, and BEM naming conventions.
/// </summary>
/// <example>
/// <code>
/// var css = new CssClassBuilder("rhx-button")
///     .AddIf("rhx-button--brand", variant == "brand")
///     .AddIf("rhx-button--large", size == "large")
///     .AddIf("rhx-button--disabled", disabled)
///     .AddIf("rhx-button--loading", loading)
///     .Build(); // "rhx-button rhx-button--brand rhx-button--large"
/// </code>
/// </example>
public sealed class CssClassBuilder
{
    private readonly List<string> _classes = [];

    /// <summary>
    /// Creates an empty CSS class builder.
    /// </summary>
    public CssClassBuilder() { }

    /// <summary>
    /// Creates a CSS class builder initialized with a base class.
    /// </summary>
    /// <param name="initialClass">The base class to start with (e.g., "rhx-button").</param>
    public CssClassBuilder(string initialClass)
    {
        if (!string.IsNullOrWhiteSpace(initialClass))
        {
            _classes.Add(initialClass);
        }
    }

    /// <summary>
    /// Adds a CSS class unconditionally.
    /// Null and whitespace-only values are silently ignored.
    /// </summary>
    /// <param name="className">The CSS class to add.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder Add(string? className)
    {
        if (!string.IsNullOrWhiteSpace(className))
        {
            _classes.Add(className);
        }
        return this;
    }

    /// <summary>
    /// Adds multiple CSS classes unconditionally.
    /// Null and whitespace-only values are silently ignored.
    /// </summary>
    /// <param name="classNames">The CSS classes to add.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddRange(params string?[] classNames)
    {
        foreach (var className in classNames)
        {
            Add(className);
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class only when the condition is true.
    /// </summary>
    /// <param name="className">The CSS class to conditionally add.</param>
    /// <param name="condition">Whether to include the class.</param>
    /// <returns>This builder for chaining.</returns>
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
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="trueClass">The class to add when condition is true.</param>
    /// <param name="falseClass">The class to add when condition is false.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddChoice(bool condition, string trueClass, string falseClass)
    {
        _classes.Add(condition ? trueClass : falseClass);
        return this;
    }

    /// <summary>
    /// Adds a BEM variant class "rhx-{block}--{variant}" if variant is non-null/empty.
    /// </summary>
    /// <param name="block">The BEM block name (e.g., "button").</param>
    /// <param name="variant">The variant name (e.g., "brand", "danger"), or null to skip.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddVariant(string block, string? variant)
    {
        if (!string.IsNullOrWhiteSpace(variant))
        {
            _classes.Add($"rhx-{block}--{variant}");
        }
        return this;
    }

    /// <summary>
    /// Adds a BEM size class "rhx-{block}--{size}" if size is non-null/empty.
    /// </summary>
    /// <param name="block">The BEM block name (e.g., "button").</param>
    /// <param name="size">The size name (e.g., "small", "large"), or null to skip.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddSize(string block, string? size)
    {
        if (!string.IsNullOrWhiteSpace(size))
        {
            _classes.Add($"rhx-{block}--{size}");
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class derived from a nullable enum value, formatted as lowercase.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="prefix">The class prefix (e.g., "rhx-button--").</param>
    /// <param name="value">The enum value, or null to skip.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddEnum<T>(string prefix, T? value) where T : struct, Enum
    {
        if (value.HasValue)
        {
            _classes.Add($"{prefix}{value.Value.ToString().ToLowerInvariant()}");
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class from a callback that receives this builder's block context.
    /// Useful for complex conditional logic that doesn't fit the simpler methods.
    /// </summary>
    /// <param name="factory">A factory function that returns a class name or null.</param>
    /// <returns>This builder for chaining.</returns>
    public CssClassBuilder AddFrom(Func<string?> factory)
    {
        var className = factory();
        if (!string.IsNullOrWhiteSpace(className))
        {
            _classes.Add(className);
        }
        return this;
    }

    /// <summary>
    /// Builds the final space-separated CSS class string.
    /// </summary>
    /// <returns>A string of space-separated CSS classes, or empty string if none.</returns>
    public string Build() => string.Join(" ", _classes);

    /// <summary>
    /// Returns true if no classes have been added.
    /// </summary>
    public bool IsEmpty => _classes.Count == 0;

    /// <summary>
    /// Returns the number of classes currently in the builder.
    /// </summary>
    public int Count => _classes.Count;

    /// <inheritdoc/>
    public override string ToString() => Build();
}
