# RazorHX

A complete ASP.NET Core UI component library implemented as Razor Tag Helpers with first-class htmx integration. Every component renders clean, semantic HTML styled by RazorHX's own CSS design system. No external component library dependencies.

## Quick Start

### 1. Install the package

```bash
dotnet add package RazorHX
```

### 2. Register services

```csharp
builder.Services.AddRazorHX(options =>
{
    options.DefaultTheme = "light";
    options.IncludeHtmxScript = true;
});
```

### 3. Add middleware

```csharp
app.UseRazorHX();
```

### 4. Add the tag helper

In your `_ViewImports.cshtml`:

```razor
@addTagHelper *, RazorHX
```

### 5. Use components

```html
<rhx-button variant="Brand" hx-post="/api/submit" hx-target="#result">
    Submit
</rhx-button>
```

## Solution Structure

| Project | Description |
|---------|-------------|
| `RazorHX` | Core tag helper library + embedded CSS/JS assets |
| `RazorHX.Demo` | Demo/documentation site showcasing components |
| `RazorHX.Tests` | Unit tests for tag helper rendering |

## Design System

RazorHX owns its entire rendering and styling stack using:

- **CSS Custom Properties** for design tokens (colors, spacing, typography, etc.)
- **BEM naming** with `rhx-` prefix (e.g., `rhx-button`, `rhx-button--brand`, `rhx-button__icon`)
- **Light/dark themes** via `data-rhx-theme` attribute
- **Utility classes** for layout and spacing (`rhx-flex`, `rhx-gap-md`, `rhx-p-lg`, etc.)

## Building

```bash
dotnet build
dotnet test
dotnet run --project RazorHX.Demo
```

## License

MIT
