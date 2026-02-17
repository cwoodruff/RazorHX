using RazorHX.Components.Imagery;
using Xunit;

namespace RazorHX.Tests;

public class AvatarTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private AvatarTagHelper CreateHelper()
    {
        var helper = new AvatarTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar"));
    }

    [Fact]
    public void Has_Role_Img()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        AssertAttribute(output, "role", "img");
    }

    [Fact]
    public void Label_Sets_AriaLabel()
    {
        var helper = CreateHelper();
        helper.Label = "Jane Doe";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Jane Doe");
    }

    // ══════════════════════════════════════════════
    //  Image
    // ══════════════════════════════════════════════

    [Fact]
    public void Image_Renders_Img()
    {
        var helper = CreateHelper();
        helper.Image = "/photo.jpg";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-avatar__image", content);
        Assert.Contains("src=\"/photo.jpg\"", content);
    }

    [Fact]
    public void Image_Has_Lazy_Loading()
    {
        var helper = CreateHelper();
        helper.Image = "/photo.jpg";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("loading=\"lazy\"", content);
    }

    [Fact]
    public void Image_Eager_Loading()
    {
        var helper = CreateHelper();
        helper.Image = "/photo.jpg";
        helper.Loading = "eager";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("loading=\"eager\"", content);
    }

    [Fact]
    public void Image_Has_Empty_Alt()
    {
        var helper = CreateHelper();
        helper.Image = "/photo.jpg";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("alt=\"\"", content);
    }

    // ══════════════════════════════════════════════
    //  Initials
    // ══════════════════════════════════════════════

    [Fact]
    public void Initials_Renders_Span()
    {
        var helper = CreateHelper();
        helper.Initials = "JD";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-avatar__initials", content);
        Assert.Contains("JD", content);
    }

    [Fact]
    public void Initials_Has_Hash_Attribute()
    {
        var helper = CreateHelper();
        helper.Initials = "JD";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("data-rhx-hash=", content);
    }

    [Fact]
    public void Initials_Hash_Deterministic()
    {
        var hash1 = AvatarTagHelper.GetInitialsHash("JD");
        var hash2 = AvatarTagHelper.GetInitialsHash("JD");
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Initials_Hash_In_Range()
    {
        var hash = AvatarTagHelper.GetInitialsHash("AB");
        Assert.InRange(hash, 0, 7);
    }

    [Fact]
    public void Image_Preferred_Over_Initials()
    {
        var helper = CreateHelper();
        helper.Image = "/photo.jpg";
        helper.Initials = "JD";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-avatar__image", content);
        Assert.DoesNotContain("rhx-avatar__initials", content);
    }

    // ══════════════════════════════════════════════
    //  Shape
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Shape_Circle()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--circle"));
    }

    [Fact]
    public void Shape_Square()
    {
        var helper = CreateHelper();
        helper.Shape = "square";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--square"));
    }

    [Fact]
    public void Shape_Rounded()
    {
        var helper = CreateHelper();
        helper.Shape = "rounded";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--rounded"));
    }

    // ══════════════════════════════════════════════
    //  Size
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Size_Medium()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--medium"));
    }

    [Fact]
    public void Size_Small()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--small"));
    }

    [Fact]
    public void Size_Large()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-avatar--large"));
    }

    [Fact]
    public void Custom_Size_Uses_Style()
    {
        var helper = CreateHelper();
        helper.Size = "4rem";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-avatar--4rem"));
        AssertAttribute(output, "style", "--rhx-avatar-size: 4rem");
    }

    // ══════════════════════════════════════════════
    //  Custom CSS & htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Custom_CssClass()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-avatar";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-avatar"));
        Assert.True(HasClass(output, "rhx-avatar"));
    }

    [Fact]
    public void Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/user";
        var context = CreateContext("rhx-avatar");
        var output = CreateOutput("rhx-avatar");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/user");
    }
}
