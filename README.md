# htmxRazor

[![NuGet](https://img.shields.io/nuget/v/htmxRazor.svg)](https://www.nuget.org/packages/htmxRazor)
[![CI](https://github.com/cwoodruff/RazorHX/actions/workflows/ci.yml/badge.svg)](https://github.com/cwoodruff/RazorHX/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.
microsoft.com/)

**Server-rendered UI components for ASP.NET Core with first-class htmx integration.**

htmxRazor is a complete UI component library implemented as Razor Tag Helpers. Every component renders clean, semantic HTML styled by its own CSS design system. No client-side framework required — just add htmx attributes to any component for dynamic behavior.

## Why htmxRazor?

- **Server-rendered** — No JavaScript framework. Components are Tag Helpers that render HTML on the server.
- **htmx-native** — Every component supports `hx-get`, `hx-post`, `hx-target`, `hx-swap`, and all htmx attributes directly.
- **72 components** — Buttons, forms, dialogs, tabs, trees, carousels, data visualization, and more across 10 categories.
- **Design tokens** — Light/dark themes via CSS custom properties. Toggle with `data-rhx-theme` or `RHX.toggleTheme()`.
- **Accessible** — Semantic HTML, ARIA attributes, keyboard navigation, and screen reader support built in.
- **Model binding** — Form components integrate with ASP.NET Core model binding, validation, and `ModelExpression`.
- **Zero config** — One NuGet package. Two lines of setup. Start using components immediately.

## Quick Start

### 1. Install

```bash
dotnet add package htmxRazor
```

### 2. Configure

```csharp
// Program.cs
builder.Services.AddRazorPages();
builder.Services.AddRazorHX();

var app = builder.Build();
app.UseStaticFiles();
app.UseRazorHX();       // Serves component CSS, JS, and htmx from /_rhx/
app.MapRazorPages();
app.Run();
```

### 3. Register Tag Helpers

```razor
@* _ViewImports.cshtml *@
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, RazorHX
```

### 4. Use Components

```html
<rhx-button rhx-variant="brand" rhx-size="large">
    Save Changes
</rhx-button>
```

## Feature Highlights

### Buttons with Every Variant

```html
<rhx-button rhx-variant="brand">Brand</rhx-button>
<rhx-button rhx-variant="success" rhx-appearance="outlined">Success</rhx-button>
<rhx-button rhx-variant="danger" rhx-appearance="plain">Delete</rhx-button>
<rhx-button rhx-variant="brand" rhx-pill="true">Pill Shape</rhx-button>
<rhx-button rhx-variant="brand" rhx-loading="true">Saving...</rhx-button>
```

### Form Controls with Model Binding

```html
<!-- Auto-detects type, label, validation from model metadata -->
<rhx-input rhx-for="Email" rhx-with-clear="true" />

<rhx-input rhx-label="Password"
           rhx-type="password"
           rhx-password-toggle="true"
           name="password" />
```

### htmx on Any Component

```html
<!-- Live search with debounce -->
<rhx-input rhx-label="Search"
           rhx-placeholder="Type to search..."
           hx-get="/search"
           hx-trigger="input changed delay:300ms"
           hx-target="#results" />

<!-- Delete with confirmation -->
<rhx-button rhx-variant="danger"
            hx-delete="/api/items/42"
            hx-confirm="Are you sure?"
            hx-target="#item-list"
            hx-swap="outerHTML">
    Delete
</rhx-button>
```

### Modal Dialogs

```html
<rhx-button rhx-variant="brand" data-rhx-dialog-open="edit-dialog">
    Edit Profile
</rhx-button>

<rhx-dialog id="edit-dialog" rhx-label="Edit Profile">
    <rhx-input rhx-for="DisplayName" />
    <rhx-dialog-footer>
        <rhx-button rhx-variant="neutral" rhx-appearance="outlined"
                    onclick="this.closest('dialog').close()">Cancel</rhx-button>
        <rhx-button rhx-variant="brand" type="submit">Save</rhx-button>
    </rhx-dialog-footer>
</rhx-dialog>
```

### Theming

```html
<!-- Set theme on <html> -->
<html data-rhx-theme="light">

<!-- Toggle programmatically -->
<rhx-button onclick="RHX.toggleTheme()">Toggle Theme</rhx-button>

<!-- Override any design token -->
<style>
  :root {
    --rhx-color-brand-500: #6366f1;
    --rhx-radius-md: 0.75rem;
  }
</style>
```

## Component Catalog

| Category | Components |
|----------|-----------|
| **Actions** | Button, Button Group, Dropdown |
| **Forms** | Input, Textarea, Select, Combobox, Checkbox, Switch, Radio, Slider, Rating, Color Picker, File Input, Number Input |
| **Feedback** | Callout, Badge, Tag, Spinner, Skeleton, Progress Bar, Progress Ring, Tooltip |
| **Navigation** | Tabs, Breadcrumb, Tree, Carousel |
| **Organization** | Card, Divider, Split Panel, Scroller |
| **Overlays** | Dialog, Drawer, Details |
| **Imagery** | Icon (43 built-in), Avatar, Animated Image, Comparison, Zoomable Frame |
| **Formatting** | Format Bytes, Format Date, Format Number, Relative Time |
| **Utilities** | Copy Button, QR Code, Animation, Popup, Popover |
| **Patterns** | Active Search, Infinite Scroll, Lazy Load, Poll |

## How It Compares

| | RazorHX | Bootstrap | Blazor | Web Awesome |
|---|---------|-----------|--------|-------------|
| **Rendering** | Server (Tag Helpers) | Client (jQuery/JS) | Client (WebAssembly/SignalR) | Client (Web Components) |
| **htmx support** | Native on every component | Manual attribute wiring | N/A (own reactivity) | Manual attribute wiring |
| **Bundle size** | CSS + htmx (~14 KB gzip) | ~24 KB JS + ~22 KB CSS | ~2 MB WebAssembly runtime | ~80 KB JS |
| **Model binding** | ASP.NET Core `ModelExpression` | None | Blazor `@bind` | None |
| **Dependencies** | ASP.NET Core | jQuery | .NET runtime in browser | Lit |
| **Theming** | CSS custom properties | Sass variables | CSS isolation | CSS custom properties |

## Solution Structure

| Project | Description |
|---------|-------------|
| `RazorHX` | Core Tag Helper library with embedded CSS/JS assets |
| `RazorHX.Demo` | Documentation site showcasing all components |
| `RazorHX.Tests` | 1,436 unit tests for Tag Helper rendering |

## Development

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run the demo site
dotnet run --project RazorHX.Demo

# Pack for NuGet
dotnet pack RazorHX/RazorHX.csproj --configuration Release
```

## Design System

RazorHX owns its entire rendering and styling stack:

- **CSS Custom Properties** for design tokens (colors, spacing, typography, borders, shadows)
- **BEM naming** with `rhx-` prefix (`rhx-button`, `rhx-button--brand`, `rhx-button__label`)
- **Light/dark themes** via `data-rhx-theme` attribute on `<html>`
- **Utility classes** for layout and spacing (`rhx-flex`, `rhx-gap-md`, `rhx-p-lg`)
- **Auto-injected assets** — `AddRazorHX()` injects tokens, reset, core, and utilities CSS plus the htmx script into `<head>`

## Roadmap

| Version | Scope |
|---------|-------|
| **v0.1.0** | Button, Input, Validation — minimum viable library |
| **v0.2.0** | All Form Controls + Feedback components |
| **v0.3.0** | Navigation + Organization components |
| **v0.4.0** | Imagery + Utilities + Data Visualization |
| **v0.5.0** | Composite Patterns (infinite scroll, search, poll, lazy load) |
| **v1.0.0** | Full catalog, stable API, comprehensive docs, theme system |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development setup, PR guidelines, and coding standards.

## License

[MIT](LICENSE) - Copyright (c) 2024 Chris Woody Woodruff
