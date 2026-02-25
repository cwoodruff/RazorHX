using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class ValidationSummaryTagHelperTests : TagHelperTestBase
{
    private static ValidationSummaryTagHelper CreateHelper() => new();

    // ── With errors ──

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Callout_Class()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-callout"));
    }

    [Fact]
    public void Default_Variant_Is_Danger()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-callout--danger"));
    }

    [Fact]
    public void Has_Role_Alert()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        AssertAttribute(output, "role", "alert");
    }

    [Fact]
    public void Has_Error_List()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-validation-summary__list", content);
        Assert.Contains("<li>Email is required.</li>", content);
    }

    [Fact]
    public void Lists_All_Errors()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        viewContext.ModelState.AddModelError("Password", "Password must be at least 8 characters.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Email is required.", content);
        Assert.Contains("Password must be at least 8 characters.", content);
    }

    [Fact]
    public void Multiple_Errors_Per_Field()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        viewContext.ModelState.AddModelError("Email", "Email format invalid.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Email is required.", content);
        Assert.Contains("Email format invalid.", content);
    }

    [Fact]
    public void Has_Icon()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-callout__icon", content);
        Assert.Contains("<svg", content);
    }

    // ── Without errors ──

    [Fact]
    public void No_Errors_Suppresses_Output()
    {
        var helper = CreateHelper();
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.True(output.IsContentModified || output.Content.IsEmptyOrWhiteSpace);
        // Verify the tag itself is suppressed
        Assert.Null(output.TagName);
    }

    // ── Custom variant ──

    [Fact]
    public void Custom_Variant_Warning()
    {
        var helper = CreateHelper();
        helper.Variant = "warning";
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Check email.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-callout--warning"));
    }

    // ── Custom CSS class ──

    [Fact]
    public void Custom_CssClass_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-summary";
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Error.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-callout"));
        Assert.True(HasClass(output, "my-summary"));
    }

    // ── HTML encoding ──

    [Fact]
    public void Error_Messages_Are_Html_Encoded()
    {
        var helper = CreateHelper();
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Name", "Use <em> tags");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-summary");
        var output = CreateOutput("rhx-validation-summary");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("<em>", content);
        Assert.Contains("&lt;em&gt;", content);
    }
}
