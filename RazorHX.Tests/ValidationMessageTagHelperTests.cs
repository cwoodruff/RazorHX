using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Components.Forms;
using Xunit;

namespace RazorHX.Tests;

public class ValidationMessageTagHelperTests : TagHelperTestBase
{
    private static ValidationMessageTagHelper CreateHelper() => new();

    private class TestModel
    {
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
    }

    private static ModelExpression CreateModelExpressionFor(string propertyName)
    {
        var provider = new EmptyModelMetadataProvider();
        var metadata = provider.GetMetadataForProperty(typeof(TestModel), propertyName);
        var explorer = new ModelExplorer(provider, metadata, null);
        return new ModelExpression(propertyName, explorer);
    }

    // ── Element rendering ──

    [Fact]
    public void Renders_Span_Element()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();
        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();
        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-validation-message"));
    }

    [Fact]
    public void Has_Generated_Id()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();
        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertAttribute(output, "id", "Email-error");
    }

    [Fact]
    public void Has_Aria_Live()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();
        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertAttribute(output, "aria-live", "polite");
    }

    // ── With errors ──

    [Fact]
    public void Error_Shows_Error_Text()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Email is required.", content);
    }

    [Fact]
    public void Error_Has_Error_Modifier()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-validation-message--error"));
    }

    [Fact]
    public void Error_Has_Role_Alert()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertAttribute(output, "role", "alert");
    }

    [Fact]
    public void Error_Not_Hidden()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Email is required.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertNoAttribute(output, "hidden");
    }

    // ── Without errors ──

    [Fact]
    public void No_Error_Is_Hidden()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertAttribute(output, "hidden", "hidden");
    }

    [Fact]
    public void No_Error_No_Role_Alert()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertNoAttribute(output, "role");
    }

    [Fact]
    public void No_Error_No_Error_Modifier()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-validation-message--error"));
    }

    [Fact]
    public void No_Error_Empty_Content()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Empty(content);
    }

    // ── Multiple errors shows first ──

    [Fact]
    public void Multiple_Errors_Shows_First()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "First error.");
        viewContext.ModelState.AddModelError("Email", "Second error.");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("First error.", content);
        Assert.DoesNotContain("Second error.", content);
    }

    // ── Custom CSS class ──

    [Fact]
    public void Custom_CssClass_Merged()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        helper.CssClass = "my-error";
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-validation-message"));
        Assert.True(HasClass(output, "my-error"));
    }

    // ── HTML encoding ──

    [Fact]
    public void Error_Message_Is_Html_Encoded()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Email");
        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("Email", "Use <strong> tags");
        helper.ViewContext = viewContext;

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("<strong>", content);
        Assert.Contains("&lt;strong&gt;", content);
    }

    // ── Id sanitization ──

    [Fact]
    public void Nested_Property_Id_Sanitized()
    {
        var helper = CreateHelper();
        // Simulate a nested property like "Address.City"
        var provider = new EmptyModelMetadataProvider();
        var metadata = provider.GetMetadataForProperty(typeof(TestModel), "Email");
        var explorer = new ModelExplorer(provider, metadata, null);
        helper.For = new ModelExpression("Address.City", explorer);
        helper.ViewContext = CreateViewContext();

        var context = CreateContext("rhx-validation-message");
        var output = CreateOutput("rhx-validation-message");

        helper.Process(context, output);

        AssertAttribute(output, "id", "Address_City-error");
    }
}
