using htmxRazor.Components.Overlays;
using Xunit;

namespace htmxRazor.Tests;

public class CommandGroupTagHelperTests : TagHelperTestBase
{
    [Fact]
    public async Task Renders_Div_With_Group_Role()
    {
        var helper = new CommandGroupTagHelper { Heading = "Pages" };
        var context = CreateContext("rhx-command-group");
        var output = CreateOutput("rhx-command-group", childContent: "<div>items</div>");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
        AssertAttribute(output, "role", "group");
    }

    [Fact]
    public async Task Has_Group_Class()
    {
        var helper = new CommandGroupTagHelper { Heading = "Pages" };
        var context = CreateContext("rhx-command-group");
        var output = CreateOutput("rhx-command-group", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "class", "rhx-command-palette__group");
    }

    [Fact]
    public async Task Heading_Rendered_With_AriaLabelledby()
    {
        var helper = new CommandGroupTagHelper { Heading = "Settings" };
        var context = CreateContext("rhx-command-group");
        var output = CreateOutput("rhx-command-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Settings", content);
        Assert.Contains("rhx-command-palette__group-heading", content);
        Assert.Contains("role=\"presentation\"", content);

        // aria-labelledby on the group should reference heading ID
        var ariaLabelledby = GetAttribute(output, "aria-labelledby");
        Assert.NotNull(ariaLabelledby);
        Assert.Contains(ariaLabelledby!, content); // heading ID exists in content
    }

    [Fact]
    public async Task Heading_Is_Html_Encoded()
    {
        var helper = new CommandGroupTagHelper { Heading = "Tom & Jerry" };
        var context = CreateContext("rhx-command-group");
        var output = CreateOutput("rhx-command-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Tom &amp; Jerry", content);
    }
}
