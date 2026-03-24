using htmxRazor.Components.Overlays;
using htmxRazor.Rendering;
using Xunit;

namespace htmxRazor.Tests;

public class DialogTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private DialogTagHelper CreateHelper()
    {
        var helper = new DialogTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  DialogTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Dialog_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("dialog", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dialog"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-dialog", "");
    }

    [Fact]
    public async Task Contains_Panel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dialog__panel", content);
    }

    [Fact]
    public async Task Contains_Body()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dialog__body", content);
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-dialog";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-dialog"));
        Assert.True(HasClass(output, "rhx-dialog"));
    }

    // ══════════════════════════════════════════════
    //  Header & Label
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Label_Renders_Header()
    {
        var helper = CreateHelper();
        helper.Label = "Edit Item";
        helper.Id = "edit-dialog";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dialog__header", content);
        Assert.Contains("rhx-dialog__title", content);
        Assert.Contains("Edit Item", content);
    }

    [Fact]
    public async Task Label_Sets_AriaLabelledby()
    {
        var helper = CreateHelper();
        helper.Label = "Edit Item";
        helper.Id = "edit-dialog";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-labelledby", "edit-dialog-title");
    }

    [Fact]
    public async Task Title_Id_Matches_AriaLabelledby()
    {
        var helper = CreateHelper();
        helper.Label = "Edit Item";
        helper.Id = "edit-dialog";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("id=\"edit-dialog-title\"", content);
    }

    [Fact]
    public async Task Header_Has_Close_Button()
    {
        var helper = CreateHelper();
        helper.Label = "Title";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dialog__close", content);
        Assert.Contains("aria-label=\"Close\"", content);
    }

    [Fact]
    public async Task Label_Is_HtmlEncoded()
    {
        var helper = CreateHelper();
        helper.Label = "Tom & Jerry";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Tom &amp; Jerry", content);
    }

    [Fact]
    public async Task No_Label_No_Header()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-dialog__header", content);
    }

    [Fact]
    public async Task NoHeader_Suppresses_Header()
    {
        var helper = CreateHelper();
        helper.Label = "Title";
        helper.NoHeader = true;
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-dialog__header", content);
    }

    [Fact]
    public async Task NoHeader_No_AriaLabelledby()
    {
        var helper = CreateHelper();
        helper.Label = "Title";
        helper.NoHeader = true;
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "aria-labelledby");
    }

    // ══════════════════════════════════════════════
    //  Open state
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Open_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Open = true;
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "open", "open");
    }

    [Fact]
    public async Task Not_Open_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "open");
    }

    // ══════════════════════════════════════════════
    //  Footer slot
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Footer_Slot_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-dialog",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("footer", "<button>Save</button>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("rhx-dialog__footer", html);
        Assert.Contains("<button>Save</button>", html);
    }

    [Fact]
    public async Task No_Footer_Slot_No_Footer()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-dialog__footer", content);
    }

    [Fact]
    public async Task Footer_After_Body()
    {
        var helper = CreateHelper();
        helper.Label = "Title";
        var context = CreateContext("rhx-dialog");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-dialog",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("footer", "<button>OK</button>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                content.AppendHtml("<p>Body</p>");
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var bodyIdx = html.IndexOf("rhx-dialog__body");
        var footerIdx = html.IndexOf("rhx-dialog__footer");
        Assert.True(bodyIdx < footerIdx);
    }

    // ══════════════════════════════════════════════
    //  DialogFooterTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Footer_Suppresses_Output()
    {
        var footerHelper = new DialogFooterTagHelper();
        var context = CreateContext("rhx-dialog-footer");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-dialog-footer", childContent: "<button>OK</button>");

        await footerHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task Footer_Registers_Slot()
    {
        var footerHelper = new DialogFooterTagHelper();
        var context = CreateContext("rhx-dialog-footer");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-dialog-footer", childContent: "<button>OK</button>");

        await footerHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("footer"));
    }

    // ══════════════════════════════════════════════
    //  Size
    // ══════════════════════════════════════════════

    [Theory]
    [InlineData("small")]
    [InlineData("medium")]
    [InlineData("large")]
    [InlineData("full")]
    public async Task Size_Preset_Adds_Modifier_Class(string size)
    {
        var helper = CreateHelper();
        helper.Size = size;
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, $"rhx-dialog--{size}"));
    }

    [Fact]
    public async Task Size_Custom_Value_Adds_Custom_Class()
    {
        var helper = CreateHelper();
        helper.Size = "40rem";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dialog--custom"));
    }

    [Fact]
    public async Task Size_Custom_Value_Sets_CssProperty()
    {
        var helper = CreateHelper();
        helper.Size = "40rem";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        var style = GetAttribute(output, "style");
        Assert.Contains("--rhx-dialog-width: 40rem", style);
    }

    [Fact]
    public async Task Size_Null_No_Size_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-dialog--small"));
        Assert.False(HasClass(output, "rhx-dialog--medium"));
        Assert.False(HasClass(output, "rhx-dialog--large"));
        Assert.False(HasClass(output, "rhx-dialog--full"));
        Assert.False(HasClass(output, "rhx-dialog--custom"));
    }

    [Fact]
    public async Task Size_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Size = "Large";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dialog--large"));
    }

    // ══════════════════════════════════════════════
    //  htmx support
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/content";
        helper.HxTarget = "#body";
        var context = CreateContext("rhx-dialog");
        var output = CreateOutput("rhx-dialog", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/content");
        AssertAttribute(output, "hx-target", "#body");
    }
}
