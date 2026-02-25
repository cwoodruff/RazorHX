# Contributing to htmxRazor

Thank you for your interest in contributing to htmxRazor! This guide will help you get set up and familiar with the project's conventions.

## Development Setup

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- A code editor (VS Code, Visual Studio, Rider)

### Getting Started

```bash
# Clone the repository
git clone https://github.com/cwoodruff/htmxRazor.git
cd htmxRazor

# Build
dotnet build

# Run tests
dotnet test

# Run the demo site
dotnet run --project htmxRazor.Demo
```

The demo site will be available at `https://localhost:5001` (or the port shown in the console).

## Project Structure

| Project | Purpose |
|---------|---------|
| `htmxRazor/` | Core library — Tag Helpers, CSS, JS, infrastructure |
| `htmxRazor.Demo/` | Documentation and demo site |
| `htmxRazor.Tests/` | Unit tests |

### Key Directories

```
htmxRazor/
├── Assets/css/components/    # Component stylesheets
├── Assets/css/themes/        # Light/dark theme files
├── Assets/js/components/     # Component JavaScript
├── Components/               # Tag Helper implementations
│   ├── Actions/
│   ├── Forms/
│   ├── Feedback/
│   ├── Navigation/
│   ├── Organization/
│   ├── Overlays/
│   ├── Imagery/
│   ├── Formatting/
│   ├── Utilities/
│   └── Patterns/
├── Infrastructure/           # Base classes, CSS builder, helpers
└── Rendering/                # Slot rendering, asset injection
```

## Coding Standards

### Tag Helper Conventions

- **Class name**: `{ComponentName}TagHelper` (e.g., `ButtonTagHelper`)
- **Tag name**: `rhx-{component}` (e.g., `<rhx-button>`)
- **Attribute prefix**: `rhx-` for component properties (e.g., `rhx-variant`, `rhx-size`)
- **Base class**: Extend `htmxRazorTagHelperBase` (or `FormControlTagHelperBase` for form controls)
- **Block name**: Override `BlockName` to return the BEM block (e.g., `"button"`)

### CSS Conventions

- **BEM naming**: `.rhx-{block}`, `.rhx-{block}__{element}`, `.rhx-{block}--{modifier}`
- **Design tokens**: Use `var(--rhx-*)` custom properties, never hardcoded values
- **Theme support**: All colors must work in both light and dark themes
- **File per component**: One CSS file per component in `Assets/css/components/`

### C# Style

- Use XML documentation on all public types and members
- Follow the existing patterns — look at `ButtonTagHelper` as the reference implementation
- Use `CssClassBuilder` for assembling CSS classes
- Use `ApplyBaseAttributes()` for consistent attribute handling
- Forward htmx attributes with `RenderHtmxAttributes()` or `BuildHtmxAttributeString()`

### Testing

- Every Tag Helper must have unit tests
- Test file naming: `{ComponentName}TagHelperTests.cs`
- Test the rendered HTML output, CSS classes, attributes, and ARIA properties
- Use the existing test helper patterns for creating `TagHelperContext` and `TagHelperOutput`

## Pull Request Guidelines

### Before Submitting

1. **Create an issue first** for significant changes — discuss the approach before coding
2. **Branch from `main`** — use descriptive branch names (`feat/combobox-multi-select`, `fix/dialog-focus-trap`)
3. **Write tests** — all new components and bug fixes need tests
4. **Run the full suite** — `dotnet test` must pass with 0 failures
5. **Build clean** — `dotnet build` must produce 0 errors and 0 warnings

### PR Content

- Keep PRs focused — one feature or fix per PR
- Write a clear description of what changed and why
- Include before/after screenshots for visual changes
- Reference the related issue (`Closes #123`)

### Commit Messages

Use clear, imperative commit messages:

```
Add combobox multi-select support
Fix dialog focus trap on nested dialogs
Update button loading state ARIA attributes
```

## Adding a New Component

1. **Create the Tag Helper** in the appropriate `Components/{Category}/` directory
2. **Create the CSS** in `Assets/css/components/rhx-{component}.css`
3. **Create JS** (if interactive) in `Assets/js/components/rhx-{component}.js`
4. **Add CSS/JS references** to `_Layout.cshtml` in the demo project
5. **Write unit tests** in `htmxRazor.Tests/`
6. **Add a demo page** or section in `htmxRazor.Demo/`
7. **Update the sidebar nav** in `_SidebarNav.cshtml`

## Reporting Issues

- Use the [bug report template](.github/ISSUE_TEMPLATE/bug_report.md) for bugs
- Use the [feature request template](.github/ISSUE_TEMPLATE/feature_request.md) for ideas
- Include the .NET version, browser, and OS when reporting rendering issues

## Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code.

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE).
