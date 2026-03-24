using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class WizardStepTagHelperTests : TagHelperTestBase
{
    // ── Output suppression ──

    [Fact]
    public async Task Suppresses_Output()
    {
        var helper = new WizardStepTagHelper { Title = "Account" };

        var context = CreateContext("rhx-wizard-step");
        context.Items[typeof(WizardStepList)] = new WizardStepList();
        var output = CreateOutput("rhx-wizard-step", childContent: "Step content");

        await helper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified || output.Content.IsEmptyOrWhiteSpace || output.TagName == null);
    }

    // ── Step registration ──

    [Fact]
    public async Task Registers_Step_Data()
    {
        var helper = new WizardStepTagHelper { Title = "Account" };

        var context = CreateContext("rhx-wizard-step");
        var steps = new WizardStepList();
        context.Items[typeof(WizardStepList)] = steps;
        var output = CreateOutput("rhx-wizard-step", childContent: "Step content");

        await helper.ProcessAsync(context, output);

        Assert.Single(steps);
    }

    [Fact]
    public async Task Title_Set()
    {
        var helper = new WizardStepTagHelper { Title = "Account" };

        var context = CreateContext("rhx-wizard-step");
        var steps = new WizardStepList();
        context.Items[typeof(WizardStepList)] = steps;
        var output = CreateOutput("rhx-wizard-step", childContent: "Step content");

        await helper.ProcessAsync(context, output);

        Assert.Equal("Account", steps[0].Title);
    }

    [Fact]
    public async Task Description_Set()
    {
        var helper = new WizardStepTagHelper
        {
            Title = "Account",
            Description = "Create your account"
        };

        var context = CreateContext("rhx-wizard-step");
        var steps = new WizardStepList();
        context.Items[typeof(WizardStepList)] = steps;
        var output = CreateOutput("rhx-wizard-step", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.Equal("Create your account", steps[0].Description);
    }

    [Fact]
    public async Task Status_Passed()
    {
        var helper = new WizardStepTagHelper
        {
            Title = "Account",
            Status = "complete"
        };

        var context = CreateContext("rhx-wizard-step");
        var steps = new WizardStepList();
        context.Items[typeof(WizardStepList)] = steps;
        var output = CreateOutput("rhx-wizard-step", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.Equal("complete", steps[0].Status);
    }
}
