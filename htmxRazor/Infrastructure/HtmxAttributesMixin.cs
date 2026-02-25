namespace htmxRazor.Infrastructure;

/// <summary>
/// Marker interface for htmxRazor components that support htmx attributes.
/// All components inheriting from <see cref="htmxRazorTagHelperBase"/> already have
/// htmx attribute support built in. This interface exists for documentation purposes
/// and for any external tooling that needs to identify htmx-capable components.
/// </summary>
/// <remarks>
/// htmx attributes are handled directly by <see cref="htmxRazorTagHelperBase"/>,
/// which provides all hx-* properties and the <see cref="htmxRazorTagHelperBase.RenderHtmxAttributes"/>
/// method. Components do not need to implement this interface — simply call
/// <c>RenderHtmxAttributes(output)</c> in their <c>Process</c> method.
/// </remarks>
public interface IHtmxSupported;
