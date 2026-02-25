using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class BreadcrumbTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private BreadcrumbTagHelper CreateHelper()
    {
        var helper = new BreadcrumbTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  BreadcrumbTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Nav_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("nav", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-breadcrumb"));
    }

    [Fact]
    public async Task Default_AriaLabel_Is_Breadcrumb()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Breadcrumb");
    }

    [Fact]
    public async Task Custom_AriaLabel()
    {
        var helper = CreateHelper();
        helper.Label = "Navigation";
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Navigation");
    }

    [Fact]
    public async Task Contains_Ol_With_List_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<ol class=\"rhx-breadcrumb__list\">", content);
        Assert.Contains("</ol>", content);
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-breadcrumb";
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-breadcrumb"));
        Assert.True(HasClass(output, "rhx-breadcrumb"));
    }

    // ══════════════════════════════════════════════
    //  Model-bound Items
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Items_Renders_Links()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Products", "/products"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("href=\"/\"", content);
        Assert.Contains(">Home</a>", content);
        Assert.Contains("href=\"/products\"", content);
        Assert.Contains(">Products</a>", content);
    }

    [Fact]
    public async Task Items_Last_Has_AriaCurrent()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-current=\"page\"", content);
    }

    [Fact]
    public async Task Items_Last_Without_Href_Is_Span()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<span class=\"rhx-breadcrumb__current\">Widget</span>", content);
    }

    [Fact]
    public async Task Items_Last_With_Href_Is_Link()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Products", "/products")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Last item has aria-current but is still a link
        Assert.Contains("aria-current=\"page\"", content);
        Assert.Contains("href=\"/products\"", content);
    }

    [Fact]
    public async Task Items_Have_Link_Class()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-breadcrumb__link\"", content);
    }

    [Fact]
    public async Task Items_Have_Item_Class()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-breadcrumb__item\"", content);
    }

    // ══════════════════════════════════════════════
    //  Separator
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Separator()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-breadcrumb__separator\"", content);
        Assert.Contains(">/</span>", content);
    }

    [Fact]
    public async Task Custom_Separator()
    {
        var helper = CreateHelper();
        helper.Separator = "›";
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(">›</span>", content);
    }

    [Fact]
    public async Task Separator_Has_AriaHidden()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    [Fact]
    public async Task Last_Item_Has_No_Separator()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Widget")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Only one separator (between Home and Widget, not after Widget)
        var separatorCount = content.Split("rhx-breadcrumb__separator").Length - 1;
        Assert.Equal(1, separatorCount);
    }

    // ══════════════════════════════════════════════
    //  Child items
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Child_Items_Render()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-breadcrumb");

        // Simulate child items registering via the getChildContentAsync callback
        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-breadcrumb",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                // Simulate breadcrumb items registering
                if (context.Items.TryGetValue("RhxBreadcrumbItems", out var obj)
                    && obj is List<(string, string?)> items)
                {
                    items.Add(("Home", "/"));
                    items.Add(("Products", "/products"));
                    items.Add(("Widget", null));
                }
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains(">Home</a>", html);
        Assert.Contains(">Products</a>", html);
        Assert.Contains(">Widget</span>", html);
        Assert.Contains("aria-current=\"page\"", html);
    }

    // ══════════════════════════════════════════════
    //  BreadcrumbItemTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Item_Suppresses_Output()
    {
        var itemHelper = new BreadcrumbItemTagHelper();
        itemHelper.Href = "/products";
        var context = CreateContext("rhx-breadcrumb-item");
        context.Items["RhxBreadcrumbItems"] = new List<(string, string?)>();
        var output = CreateOutput("rhx-breadcrumb-item", childContent: "Products");

        await itemHelper.ProcessAsync(context, output);

        Assert.Null(output.TagName);
    }

    [Fact]
    public async Task Item_Registers_With_Parent()
    {
        var itemHelper = new BreadcrumbItemTagHelper();
        itemHelper.Href = "/products";
        var context = CreateContext("rhx-breadcrumb-item");
        var items = new List<(string, string?)>();
        context.Items["RhxBreadcrumbItems"] = items;
        var output = CreateOutput("rhx-breadcrumb-item", childContent: "Products");

        await itemHelper.ProcessAsync(context, output);

        Assert.Single(items);
        Assert.Equal("Products", items[0].Item1);
        Assert.Equal("/products", items[0].Item2);
    }

    [Fact]
    public async Task Item_Without_Href()
    {
        var itemHelper = new BreadcrumbItemTagHelper();
        var context = CreateContext("rhx-breadcrumb-item");
        var items = new List<(string, string?)>();
        context.Items["RhxBreadcrumbItems"] = items;
        var output = CreateOutput("rhx-breadcrumb-item", childContent: "Current Page");

        await itemHelper.ProcessAsync(context, output);

        Assert.Single(items);
        Assert.Equal("Current Page", items[0].Item1);
        Assert.Null(items[0].Item2);
    }

    [Fact]
    public async Task Items_Label_Is_HtmlEncoded()
    {
        var helper = CreateHelper();
        helper.Items = new List<BreadcrumbItem>
        {
            new("Tom & Jerry", "/t&j"),
            new("Current")
        };
        var context = CreateContext("rhx-breadcrumb");
        var output = CreateOutput("rhx-breadcrumb", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Tom &amp; Jerry", content);
    }
}
