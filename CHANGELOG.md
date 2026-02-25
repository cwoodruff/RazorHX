# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Complete Tag Helper component library with 72 components across 10 categories
- CSS design system with light/dark themes via CSS custom properties
- First-class htmx attribute support on every component
- ASP.NET Core model binding integration for form controls
- Embedded CSS/JS assets served from `/_rhx/` middleware path
- Auto-injection of design tokens, reset CSS, and htmx script via `AddhtmxRazor()`
- 1,436 unit tests for Tag Helper rendering
- Documentation site with Getting Started guide and component reference pages
- GitHub Actions CI/CD workflows (build, release, demo deployment)

### Components by Category
- **Actions**: Button, Button Group, Dropdown
- **Forms**: Input, Textarea, Select, Combobox, Checkbox, Switch, Radio, Slider, Rating, Color Picker, File Input, Number Input
- **Feedback**: Callout, Badge, Tag, Spinner, Skeleton, Progress Bar, Progress Ring, Tooltip
- **Navigation**: Tabs, Breadcrumb, Tree, Carousel
- **Organization**: Card, Divider, Split Panel, Scroller
- **Overlays**: Dialog, Drawer, Details
- **Imagery**: Icon (43 built-in), Avatar, Animated Image, Comparison, Zoomable Frame
- **Formatting**: Format Bytes, Format Date, Format Number, Relative Time
- **Utilities**: Copy Button, QR Code, Animation, Popup, Popover
- **Patterns**: Active Search, Infinite Scroll, Lazy Load, Poll

## Milestones

### v0.1.0 — Minimum Viable Library
- Button, Input, Validation
- Core design system (tokens, reset, utilities)
- htmx integration infrastructure
- NuGet package with embedded assets

### v0.2.0 — Form Controls + Feedback
- All form components (Textarea, Select, Combobox, Checkbox, Switch, Radio, Slider, Rating, Color Picker, File Input, Number Input)
- Feedback components (Callout, Badge, Tag, Spinner, Skeleton, Progress Bar, Progress Ring, Tooltip)

### v0.3.0 — Navigation + Organization
- Tabs, Breadcrumb, Tree, Carousel
- Card, Divider, Split Panel, Scroller

### v0.4.0 — Imagery + Utilities + Data Viz
- Icon, Avatar, Animated Image, Comparison, Zoomable Frame
- Copy Button, QR Code, Animation, Popup, Popover
- Sparkline

### v0.5.0 — Composite Patterns
- Active Search, Infinite Scroll, Lazy Load, Poll

### v1.0.0 — Stable Release
- Full component catalog with stable API
- Comprehensive documentation site
- Theme customization system
- Accessibility audit complete
