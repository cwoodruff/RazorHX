using htmxRazor.Components.Feedback;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace htmxRazor.Tests;

/// <summary>
/// Tests for hx-on:* dictionary attribute support on htmxRazorTagHelperBase.
/// </summary>
public class HxOnDictionaryTests : TagHelperTestBase
{
    private CalloutTagHelper CreateHelper()
    {
        var helper = new CalloutTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public async Task Renders_No_HxOn_When_Dictionary_Is_Empty()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-on:after-request");
        AssertNoAttribute(output, "hx-on:before-request");
    }

    [Fact]
    public async Task Renders_Single_HxOn_Event()
    {
        var helper = CreateHelper();
        helper.HxOn["after-request"] = "this.remove()";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-on:after-request", "this.remove()");
    }

    [Fact]
    public async Task Renders_Multiple_HxOn_Events()
    {
        var helper = CreateHelper();
        helper.HxOn["after-request"] = "this.remove()";
        helper.HxOn["before-send"] = "console.log('sending')";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-on:after-request", "this.remove()");
        AssertAttribute(output, "hx-on:before-send", "console.log('sending')");
    }
}
