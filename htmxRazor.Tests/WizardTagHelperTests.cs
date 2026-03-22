using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using htmxRazor.Components.Navigation;
using Moq;
using Xunit;

namespace htmxRazor.Tests;

public class WizardTagHelperTests : TagHelperTestBase
{
    private WizardTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new WizardTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    /// <summary>
    /// Creates a URL helper factory that includes route values in the generated URL,
    /// so tests can verify that correct step/handler values are passed.
    /// </summary>
    private static WizardTagHelper CreateHelperWithRouteCapture()
    {
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.SetupGet(h => h.ActionContext)
            .Returns(new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()));

        urlHelper.Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns<UrlRouteContext>(ctx =>
            {
                if (ctx.Values is RouteValueDictionary rv)
                {
                    var qs = string.Join("&", rv.Select(kv => $"{kv.Key}={kv.Value}"));
                    return $"/Wizard?{qs}";
                }
                return "/Wizard";
            });

        var factory = new Mock<IUrlHelperFactory>();
        factory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper.Object);

        var helper = new WizardTagHelper(factory.Object);
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    /// <summary>
    /// Pre-populates wizard step list in the context to simulate child step processing.
    /// </summary>
    private static void RegisterSteps(TagHelperContext context, params WizardStepData[] steps)
    {
        var stepList = new WizardStepList();
        stepList.AddRange(steps);
        context.Items[typeof(WizardStepList)] = stepList;
    }

    private static WizardStepData Step(string title, string status = "incomplete") =>
        new() { Title = title, Status = status, Content = new DefaultTagHelperContent().SetContent($"Content for {title}") };

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account"), Step("Profile"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account"), Step("Profile"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-wizard"));
    }

    // ── Layout ──

    [Fact]
    public async Task Default_Layout_Horizontal()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-wizard--horizontal"));
    }

    [Fact]
    public async Task Vertical_Layout()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.Layout = "vertical";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-wizard--vertical"));
    }

    // ── ARIA ──

    [Fact]
    public async Task Has_Role_Group()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "group");
    }

    // ── Stepper ──

    [Fact]
    public async Task Renders_Stepper_Nav()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account"), Step("Profile"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__stepper", content);
        Assert.Contains("<nav", content);
    }

    [Fact]
    public async Task Renders_Steps_In_Stepper()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account"), Step("Profile"), Step("Confirm"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Account", content);
        Assert.Contains("Profile", content);
        Assert.Contains("Confirm", content);
    }

    [Fact]
    public async Task Current_Step_Marked()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B", "current"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__step--current", content);
    }

    [Fact]
    public async Task Complete_Step_Marked()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B", "current"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__step--complete", content);
    }

    [Fact]
    public async Task Current_Step_Has_AriaCurrent()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 1;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-current=\"step\"", content);
    }

    // ── Panel ──

    [Fact]
    public async Task Renders_Panel()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__panel", content);
    }

    [Fact]
    public async Task Panel_Has_Region_Role()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"region\"", content);
    }

    // ── Navigation buttons ──

    [Fact]
    public async Task ShowNav_Renders_Buttons()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__nav", content);
        Assert.Contains("Previous", content);
        Assert.Contains("Next", content);
    }

    [Fact]
    public async Task ShowNav_False_No_Buttons()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.ShowNav = false;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-wizard__nav", content);
    }

    [Fact]
    public async Task First_Step_No_Prev_Button()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 1;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("Previous", content);
    }

    [Fact]
    public async Task Last_Step_Shows_Submit()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Submit", content);
    }

    [Fact]
    public async Task Prev_Button_Uses_HxGet()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=", content);
        Assert.Contains("rhx-wizard__prev", content);
    }

    [Fact]
    public async Task Next_Button_Uses_HxPost()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 1;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=", content);
        Assert.Contains("rhx-wizard__next", content);
    }

    // ── Common attributes ──

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CssClass = "my-wizard";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-wizard"));
        Assert.True(HasClass(output, "my-wizard"));
    }

    // ── Step indicator ──

    [Fact]
    public async Task Step_Indicator_Shows_Number()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__step-indicator", content);
    }

    [Fact]
    public async Task Complete_Step_Shows_Checkmark()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<svg", content);
    }

    [Fact]
    public async Task Error_Step_Modifier()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "error"), Step("B"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-wizard__step--error", content);
    }

    [Fact]
    public async Task Linear_Prevents_Step_Click()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;
        helper.Linear = true;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Complete step indicator should be a span (not a button) when linear
        // The checkmark SVG is rendered in a <span>, not a <button>
        Assert.DoesNotContain("button class=\"rhx-wizard__step-indicator\"", content);
    }

    // ── Panel content ──

    [Fact]
    public async Task Passes_Through_Current_Step_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 1;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account"), Step("Profile"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Content for Account", content);
    }

    // ── Step navigation URL correctness ──

    [Fact]
    public async Task Next_Button_Targets_Step_2_When_On_Step_1()
    {
        var helper = CreateHelperWithRouteCapture();
        helper.Page = "/Checkout";
        helper.CurrentStep = 1;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Next button should target step 2, not step 3
        Assert.Contains("handler=WizardNext", content);
        Assert.Contains("step=2", content);
    }

    [Fact]
    public async Task Next_Button_Targets_Step_3_When_On_Step_2()
    {
        var helper = CreateHelperWithRouteCapture();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("handler=WizardNext", content);
        Assert.Contains("step=3", content);
    }

    [Fact]
    public async Task Prev_Button_Targets_Step_1_When_On_Step_2()
    {
        var helper = CreateHelperWithRouteCapture();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("A", "complete"), Step("B"), Step("C"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("handler=WizardPrev", content);
        Assert.Contains("step=1", content);
    }

    [Fact]
    public async Task Step_2_Shows_Only_Step_2_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 2;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account", "complete"), Step("Profile"), Step("Confirm"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Content for Profile", content);
        Assert.DoesNotContain("Content for Account", content);
        Assert.DoesNotContain("Content for Confirm", content);
    }

    [Fact]
    public async Task Step_3_Shows_Only_Step_3_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Checkout";
        helper.CurrentStep = 3;

        var context = CreateContext("rhx-wizard");
        RegisterSteps(context, Step("Account", "complete"), Step("Profile", "complete"), Step("Confirm"));
        var output = CreateOutput("rhx-wizard");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Content for Confirm", content);
        Assert.DoesNotContain("Content for Account", content);
        Assert.DoesNotContain("Content for Profile", content);
    }
}
