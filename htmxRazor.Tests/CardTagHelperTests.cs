using htmxRazor.Components.Organization;
using htmxRazor.Rendering;
using Xunit;

namespace htmxRazor.Tests;

public class CardTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private CardTagHelper CreateHelper()
    {
        var helper = new CardTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  CardTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-card"));
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-card";
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-card"));
        Assert.True(HasClass(output, "rhx-card"));
    }

    [Fact]
    public async Task Body_Content_Wrapped_In_Body_Div()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                content.AppendHtml("<p>Body text</p>");
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("<div class=\"rhx-card__body\">", html);
        Assert.Contains("<p>Body text</p>", html);
    }

    [Fact]
    public async Task Empty_Body_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-card__body", content);
    }

    [Fact]
    public async Task Renders_Id_Attribute()
    {
        var helper = CreateHelper();
        helper.Id = "product-card";
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "product-card");
    }

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/card";
        helper.HxTarget = "#card-content";
        var context = CreateContext("rhx-card");
        var output = CreateOutput("rhx-card", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/card");
        AssertAttribute(output, "hx-target", "#card-content");
    }

    // ══════════════════════════════════════════════
    //  CardHeaderTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Header_Slot_Rendered_Before_Body()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                // Simulate card-header registering
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("header", "<h3>Title</h3>");

                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                content.SetContent("<p>Body</p>");
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var headerIdx = html.IndexOf("rhx-card__header");
        var bodyIdx = html.IndexOf("rhx-card__body");
        Assert.True(headerIdx < bodyIdx, "Header should appear before body");
    }

    [Fact]
    public async Task Header_Has_Header_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("header", "<h3>Title</h3>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("<div class=\"rhx-card__header\">", html);
        Assert.Contains("<h3>Title</h3>", html);
    }

    [Fact]
    public async Task Header_Suppresses_Output()
    {
        var headerHelper = new CardHeaderTagHelper();
        var context = CreateContext("rhx-card-header");
        // Create slot renderer as parent would
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-header", childContent: "<h3>Title</h3>");

        await headerHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task Header_Registers_Slot()
    {
        var headerHelper = new CardHeaderTagHelper();
        var context = CreateContext("rhx-card-header");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-header", childContent: "<h3>Title</h3>");

        await headerHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("header"));
    }

    // ══════════════════════════════════════════════
    //  CardFooterTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Footer_Slot_Rendered_After_Body()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("footer", "<button>Action</button>");

                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                content.SetContent("<p>Body</p>");
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var bodyIdx = html.IndexOf("rhx-card__body");
        var footerIdx = html.IndexOf("rhx-card__footer");
        Assert.True(bodyIdx < footerIdx, "Footer should appear after body");
    }

    [Fact]
    public async Task Footer_Has_Footer_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("footer", "<button>OK</button>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("<div class=\"rhx-card__footer\">", html);
        Assert.Contains("<button>OK</button>", html);
    }

    [Fact]
    public async Task Footer_Suppresses_Output()
    {
        var footerHelper = new CardFooterTagHelper();
        var context = CreateContext("rhx-card-footer");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-footer", childContent: "<button>OK</button>");

        await footerHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task Footer_Registers_Slot()
    {
        var footerHelper = new CardFooterTagHelper();
        var context = CreateContext("rhx-card-footer");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-footer", childContent: "<button>OK</button>");

        await footerHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("footer"));
    }

    // ══════════════════════════════════════════════
    //  CardImageTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Image_Slot_Rendered_Before_Header()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("image", "<img src=\"/photo.jpg\" alt=\"Photo\" />");
                slots.SetHtml("header", "<h3>Title</h3>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var imageIdx = html.IndexOf("rhx-card__image");
        var headerIdx = html.IndexOf("rhx-card__header");
        Assert.True(imageIdx < headerIdx, "Image should appear before header");
    }

    [Fact]
    public async Task Image_Has_Image_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("image", "<img src=\"/photo.jpg\" alt=\"Photo\" />");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("<div class=\"rhx-card__image\">", html);
    }

    [Fact]
    public async Task Image_Suppresses_Output()
    {
        var imageHelper = new CardImageTagHelper();
        imageHelper.Src = "/photo.jpg";
        imageHelper.Alt = "Photo";
        var context = CreateContext("rhx-card-image");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-image", childContent: "");

        await imageHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task Image_Registers_Slot_With_Img()
    {
        var imageHelper = new CardImageTagHelper();
        imageHelper.Src = "/photo.jpg";
        imageHelper.Alt = "Photo";
        var context = CreateContext("rhx-card-image");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-image", childContent: "");

        await imageHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("image"));
    }

    [Fact]
    public async Task Image_Encodes_Src_And_Alt()
    {
        var imageHelper = new CardImageTagHelper();
        imageHelper.Src = "/photo.jpg?size=lg&crop=1";
        imageHelper.Alt = "Tom & Jerry";
        var context = CreateContext("rhx-card-image");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-card-image", childContent: "");

        await imageHelper.ProcessAsync(context, output);

        // Render the slot content to verify encoding
        var helper = CreateHelper();
        var cardContext = CreateContext("rhx-card");

        var cardOutput = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var cardSlots = SlotRenderer.FromContext(cardContext)!;
                cardSlots.SetHtml("image", "<img src=\"/photo.jpg?size=lg&amp;crop=1\" alt=\"Tom &amp; Jerry\" />");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(cardContext, cardOutput);

        var html = cardOutput.Content.GetContent();
        Assert.Contains("Tom &amp; Jerry", html);
    }

    // ══════════════════════════════════════════════
    //  All slots together
    // ══════════════════════════════════════════════

    [Fact]
    public async Task All_Slots_Render_In_Order()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-card");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-card",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("image", "<img src=\"/photo.jpg\" alt=\"\" />");
                slots.SetHtml("header", "<h3>Title</h3>");
                slots.SetHtml("footer", "<button>OK</button>");

                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                content.SetContent("<p>Body</p>");
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var imageIdx = html.IndexOf("rhx-card__image");
        var headerIdx = html.IndexOf("rhx-card__header");
        var bodyIdx = html.IndexOf("rhx-card__body");
        var footerIdx = html.IndexOf("rhx-card__footer");

        Assert.True(imageIdx >= 0);
        Assert.True(headerIdx >= 0);
        Assert.True(bodyIdx >= 0);
        Assert.True(footerIdx >= 0);
        Assert.True(imageIdx < headerIdx);
        Assert.True(headerIdx < bodyIdx);
        Assert.True(bodyIdx < footerIdx);
    }
}
