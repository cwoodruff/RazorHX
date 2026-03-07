using htmxRazor.Components.Feedback;
using Xunit;

namespace htmxRazor.Tests;

/// <summary>
/// Tests for View Transition support on htmxRazorTagHelperBase.
/// </summary>
public class ViewTransitionTests : TagHelperTestBase
{
    private CalloutTagHelper CreateHelper()
    {
        var helper = new CalloutTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public async Task Renders_ViewTransitionName_As_Style()
    {
        var helper = CreateHelper();
        helper.TransitionName = "my-callout";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "style", "view-transition-name: my-callout");
    }

    [Fact]
    public async Task Does_Not_Render_Style_When_No_TransitionName()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "style");
    }

    [Fact]
    public async Task Appends_Transition_True_To_HxSwap()
    {
        var helper = CreateHelper();
        helper.EnableViewTransition = true;
        helper.HxSwap = "innerHTML";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "innerHTML transition:true");
    }

    [Fact]
    public async Task Does_Not_Duplicate_Transition_True()
    {
        var helper = CreateHelper();
        helper.EnableViewTransition = true;
        helper.HxSwap = "innerHTML transition:true";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "innerHTML transition:true");
    }

    [Fact]
    public async Task Does_Not_Append_When_Transition_Disabled()
    {
        var helper = CreateHelper();
        helper.EnableViewTransition = false;
        helper.HxSwap = "innerHTML";

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task No_HxSwap_When_Transition_Enabled_But_Swap_Not_Set()
    {
        var helper = CreateHelper();
        helper.EnableViewTransition = true;

        var context = CreateContext("rhx-callout");
        var output = CreateOutput("rhx-callout", childContent: "Test");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-swap");
    }
}
