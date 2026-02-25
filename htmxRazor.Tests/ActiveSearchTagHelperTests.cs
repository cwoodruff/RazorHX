using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Patterns;
using Xunit;

namespace htmxRazor.Tests;

public class ActiveSearchTagHelperTests : TagHelperTestBase
{
    private ActiveSearchTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new ActiveSearchTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-active-search"));
    }

    // ── Inner structure ──

    [Fact]
    public void Contains_Input_Control_Structure()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-input__control", content);
        Assert.Contains("rhx-input__native", content);
    }

    [Fact]
    public void Input_Type_Is_Search()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"search\"", content);
    }

    [Fact]
    public void Sets_Default_Name()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"q\"", content);
    }

    [Fact]
    public void Custom_Name()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Name = "query";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"query\"", content);
    }

    // ── htmx attributes on input ──

    [Fact]
    public void Generates_HxGet_On_Input()
    {
        var helper = CreateHelper("/Search?handler=Results");
        helper.Page = "/Search";
        helper.PageHandler = "Results";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/Search?handler=Results\"", content);
    }

    [Fact]
    public void Sets_Default_Debounce_Trigger()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-trigger=\"input changed delay:300ms\"", content);
    }

    [Fact]
    public void Custom_Debounce()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Debounce = 500;

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("delay:500ms", content);
    }

    [Fact]
    public void Sets_HxTarget_On_Input()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Target = "#results";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-target=\"#results\"", content);
    }

    [Fact]
    public void Sets_HxInclude_This()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-include=\"this\"", content);
    }

    [Fact]
    public void Sets_Placeholder()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Placeholder = "Search items...";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("placeholder=\"Search items...\"", content);
    }

    [Fact]
    public void Sets_MinLength()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.MinLength = 3;

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("minlength=\"3\"", content);
    }

    // ── Clear button ──

    [Fact]
    public void WithClear_Renders_Clear_Button()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.WithClear = true;

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-input__clear", content);
    }

    [Fact]
    public void No_Clear_By_Default()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-input__clear", content);
    }

    // ── Indicator ──

    [Fact]
    public void Sets_Indicator()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Indicator = "#spinner";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-indicator=\"#spinner\"", content);
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Size = "small";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-active-search--small"));
    }

    [Fact]
    public void Medium_Size_No_Modifier()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.Size = "medium";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-active-search--medium"));
    }

    // ── Autocomplete ──

    [Fact]
    public void Autocomplete_Off()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("autocomplete=\"off\"", content);
    }

    // ── Custom class ──

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Search";
        helper.CssClass = "my-search";

        var context = CreateContext("rhx-active-search");
        var output = CreateOutput("rhx-active-search");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-active-search"));
        Assert.True(HasClass(output, "my-search"));
    }
}
