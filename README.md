# htmxRazor

[![NuGet](https://img.shields.io/nuget/v/htmxRazor.svg)](https://www.nuget.org/packages/htmxRazor)
[![CI](https://github.com/cwoodruff/htmxRazor/actions/workflows/ci.yml/badge.svg)](https://github.com/cwoodruff/htmxRazor/actions/workflows/ci.yml)
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
builder.Services.AddhtmxRazor();

var app = builder.Build();
app.UseStaticFiles();
app.UsehtmxRazor();       // Serves component CSS, JS, and htmx from /_rhx/
app.MapRazorPages();
app.Run();
```

### 3. Register Tag Helpers

```razor
@* _ViewImports.cshtml *@
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, htmxRazor
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

| | htmxRazor | Bootstrap | Blazor | Web Awesome |
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
| `htmxRazor` | Core Tag Helper library with embedded CSS/JS assets |
| `htmxRazor.Demo` | Documentation site showcasing all components |
| `htmxRazor.Tests` | 1,436 unit tests for Tag Helper rendering |

## Development

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run the demo site
dotnet run --project htmxRazor.Demo

# Pack for NuGet
dotnet pack htmxRazor/htmxRazor.csproj --configuration Release
```

## Design System

htmxRazor owns its entire rendering and styling stack:

- **CSS Custom Properties** for design tokens (colors, spacing, typography, borders, shadows)
- **BEM naming** with `rhx-` prefix (`rhx-button`, `rhx-button--brand`, `rhx-button__label`)
- **Light/dark themes** via `data-rhx-theme` attribute on `<html>`
- **Utility classes** for layout and spacing (`rhx-flex`, `rhx-gap-md`, `rhx-p-lg`)
- **Auto-injected assets** — `AddhtmxRazor()` injects tokens, reset, core, and utilities CSS plus the htmx script into `<head>`

The project uses a custom CSS design system — not Bootstrap or any other CSS framework.

Key details from the codebase instructions:
- BEM naming convention with an rhx- prefix (e.g., .rhx-button, .rhx-button--loading, .rhx-button__icon)
- CSS custom properties (design tokens) defined in rhx-tokens.css — things like --rhx-color-brand-500, --rhx-radius-md, --rhx-font-size-base
- Light/dark theme support via a data-rhx-theme attribute on <html>
- Each component has its own scoped CSS file in Assets/css/components/
- All CSS is embedded and served at /_rhx/ — no external dependencies
- Hardcoded values are not allowed; everything references var(--rhx-*) tokens

It's a fully self-contained design system built specifically for this component library.

## Roadmap

Features ranked by **user impact** and **implementation effort**. The library is at v1.1.1 with 72 components; the roadmap below covers what comes next.

### v1.2 — Notifications, Pagination & Quick Wins

| Feature | Description | Impact | Effort |
|---------|-------------|--------|--------|
| **Toast Notification System** | `<rhx-toast-container>` + `<rhx-toast>` with auto-dismiss, severity variants, stacking, and `aria-live` announcements. Server-side `HtmxToastExtensions.HxToast()` piggybacks OOB-swapped toasts onto any response. | Very High | Medium |
| **Pagination** | `<rhx-pagination>` with htmx-powered page navigation (`hx-get` with page parameter). Pairs with server-side paging for the most common data display pattern. | Very High | Medium |
| **ARIA Live Region Manager** | `<rhx-live-region>` wrapper for screen reader announcements on htmx swaps. Accepts `politeness` (polite/assertive) and `atomic` attributes. Paired with server-side `HtmxResponseExtensions` for announcing content changes. | Very High | Low |
| **CSS Cascade Layers** | Wrap all component CSS in `@layer rhx { }` so host app styles automatically override component styles regardless of specificity. Non-breaking CSS improvement. | High | Low |
| **View Transition Support** | `rhx-transition` and `rhx-transition-name` attributes on the tag helper base class. Automatically appends `transition:true` to `hx-swap` for smooth animated page transitions via the View Transitions API. | High | Low |
| **`hx-on:*` Dictionary Attribute** | Dictionary attribute on `htmxRazorTagHelperBase` for htmx 2.x event handler attributes (e.g., `hx-on::after-request`). Closes a gap already worked around in `InfiniteScrollTagHelper`. | High | Low |

### v1.3 — Data Table, Accessibility & Modern CSS

| Feature | Description | Impact | Effort |
|---------|-------------|--------|--------|
| **Data Table** | `<rhx-data-table>`, `<rhx-column>`, `<rhx-data-table-pagination>` with server-driven sort, filter, and pagination. Sort headers emit `hx-get` with query parameters; server returns `<tbody>` partials. Includes a `DataTableRequest` model binder for easy handler integration. | Very High | High |
| **Focus Management After Swaps** | `rhx-focus-after-swap` attribute to move focus to a configurable element after htmx content swaps. On by default for `<rhx-dialog>`, `<rhx-drawer>`, and future wizard component. Addresses WCAG 2.4.3 Focus Order. | High | Medium |
| **Command Palette** | `<rhx-command-palette>` modal search overlay activated via `Cmd+K`. Fires debounced `hx-get` to a configurable search endpoint. Results grouped with `<rhx-command-group>` and `<rhx-command-item>` with keyboard navigation. Entirely server-rendered results. | High | Medium |
| **Container Queries** | Update card, data table, dialog, and split panel CSS to use `@container` queries so components respond to their container width rather than the viewport. Pure CSS improvement. | Medium | Low |
| **Skip Nav & Landmarks** | `<rhx-skip-nav>` (visually hidden, visible on focus) and `<rhx-landmark>` for semantic landmark regions. Simple components addressing WCAG 2.4.1 Bypass Blocks. | Medium | Low |
| **APG Keyboard Audit** | Formal audit of `<rhx-tabs>`, `<rhx-tree>`, `<rhx-dropdown>`, and `<rhx-combobox>` against the W3C ARIA Authoring Practices Guide keyboard patterns. | Medium | Medium |

### v1.4 — Real-time, Wizard & Patterns

| Feature | Description | Impact | Effort |
|---------|-------------|--------|--------|
| **SSE Stream Container** | `<rhx-sse-stream>` wraps `hx-ext="sse"` + `sse-connect` into a declarative Tag Helper. Companion `HtmxSseExtensions.WriteEventAsync()` formats `IAsyncEnumerable<string>` as `text/event-stream`. Aligns with htmx 4.0's streaming-first direction. | High | Medium |
| **Multi-step Wizard** | `<rhx-wizard>`, `<rhx-wizard-step>` with visual stepper indicator, auto-generated navigation buttons with `hx-post`/`hx-get`, and server-side step state tracking. | High | High |
| **Response-Aware Form** | `<rhx-htmx-form>` that auto-configures the htmx response-targets extension (`hx-target-422`, `hx-target-5xx`) and injects error-handling JavaScript. Removes per-form boilerplate. | High | Medium |
| **Timeline** | `<rhx-timeline>`, `<rhx-timeline-item>` with variant-colored connectors, icon slots, and metadata regions. Pairs with `<rhx-infinite-scroll>` for loading additional events. Useful for audit logs, activity feeds, and order tracking. | Medium | Medium |
| **Optimistic UI** | `rhx-optimistic` attribute on `<rhx-switch>`, `<rhx-rating>`, and `<rhx-button>`. Immediately reflects visual state on click before server response; reverts on error via `htmx:responseError`. | Medium | Medium |
| **Load More Pattern** | `<rhx-load-more>` button-triggered pagination pattern. Simpler alternative to infinite scroll for content feeds. | Medium | Low |

### v2.0 — Platform & DX

| Feature | Description | Impact | Effort |
|---------|-------------|--------|--------|
| **Theme Builder** | Interactive page in the demo site with sliders and color pickers for all `--rhx-*` tokens. Generates a downloadable `rhx-theme-overrides.css` file. | High | High |
| **Interactive Component Playground** | Property toggle panel on each demo page. Users change variant, size, and state props; the server re-renders the component with updated markup via htmx. | High | High |
| **SignalR Hub Connector** | `<rhx-signalr>` Tag Helper wrapping SignalR hub connections with `hub-url`, `method`, `target`, and `swap` attributes. Server-side `HtmxSignalRExtensions` for sending HTML fragments from hubs. | Medium | High |
| **CSS Anchor Positioning** | Replace JavaScript positioning in `<rhx-tooltip>` and `<rhx-popover>` with CSS Anchor Positioning API (with JS fallback). Reduces JS footprint and improves edge-of-viewport correctness. | Medium | Medium |
| **VS Code Snippet Extension** | `.code-snippets` file with expansions for every `<rhx-*>` tag and common attribute combinations. Distributed alongside the NuGet package. | Medium | Low |
| **Kanban Board** | `<rhx-kanban>`, `<rhx-kanban-column>`, `<rhx-kanban-card>` with native HTML Drag and Drop. Card drop events fire `hx-post` to update position server-side. | Medium | High |

### Infrastructure Cleanup (ongoing)

| Item | Description |
|------|-------------|
| Implement or remove `EnableCssIsolation` | Option is defined in `htmxRazorOptions` but has no implementation |
| Remove `IHtmxSupported` marker interface | Dead code — nothing checks for it |
| Add `hx-replace-url` to base class | Already exists as a response header helper but missing from tag helper attributes |
| Adopt `HtmlRenderer` in components | Replace raw `StringBuilder` HTML construction with the fluent `HtmlRenderer` for better escaping and readability |
| WCAG 2.2 contrast audit | Verify all foreground/background token pairs in `rhx-tokens.css` meet AA contrast ratios (4.5:1 normal text, 3:1 large text) |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development setup, PR guidelines, and coding standards.

## License

[MIT](LICENSE) - Copyright (c) 2026 Chris Woody Woodruff
