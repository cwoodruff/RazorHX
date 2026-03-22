using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

public class WizardStateTests
{
    [Fact]
    public void IsFirstStep_True_When_Step1()
    {
        var state = new WizardState { CurrentStep = 1, TotalSteps = 3 };
        Assert.True(state.IsFirstStep);
    }

    [Fact]
    public void IsLastStep_True_When_AtEnd()
    {
        var state = new WizardState { CurrentStep = 3, TotalSteps = 3 };
        Assert.True(state.IsLastStep);
    }

    [Fact]
    public void MarkComplete_Records_Step()
    {
        var state = new WizardState { CurrentStep = 1, TotalSteps = 3 };
        state.MarkComplete(1);
        Assert.True(state.CompletedSteps.ContainsKey(1));
        Assert.True(state.CompletedSteps[1]);
    }

    [Fact]
    public void IsComplete_Returns_True()
    {
        var state = new WizardState { CurrentStep = 2, TotalSteps = 3 };
        state.MarkComplete(1);
        Assert.True(state.IsComplete(1));
    }

    [Fact]
    public void IsComplete_Returns_False_For_Incomplete()
    {
        var state = new WizardState { CurrentStep = 1, TotalSteps = 3 };
        Assert.False(state.IsComplete(2));
    }
}
