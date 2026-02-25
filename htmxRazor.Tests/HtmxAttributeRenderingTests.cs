using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

/// <summary>
/// Tests for htmx attribute rendering and URL generation in htmxRazorTagHelperBase.
/// </summary>
public class HtmxAttributeRenderingTests : TagHelperTestBase
{
    /// <summary>
    /// Concrete test implementation of the abstract base.
    /// </summary>
    private class TestTagHelper : htmxRazorTagHelperBase
    {
        protected override string BlockName => "test";

        public TestTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

        public TestTagHelper() : base() { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var css = CreateCssBuilder();
            ApplyBaseAttributes(output, css);
            RenderHtmxAttributes(output);
        }

        // Expose protected methods for testing
        public string TestBuildCssClass(params string?[] classes) => BuildCssClass(classes);
        public string? TestConditionalCssClass(string cls, bool cond) => ConditionalCssClass(cls, cond);
        public string? TestVariantCssClass(string block, string? variant) => VariantCssClass(block, variant);
        public string? TestSizeCssClass(string block, string? size) => SizeCssClass(block, size);
        public string? TestGenerateRouteUrl() => GenerateRouteUrl();
    }

    private TestTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new TestTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Verb attributes ──

    [Fact]
    public void Renders_HxGet_With_Direct_Url()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/data";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/data");
    }

    [Fact]
    public void Renders_HxPost_With_Direct_Url()
    {
        var helper = CreateHelper();
        helper.HxPost = "/api/submit";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hx-post", "/api/submit");
    }

    [Fact]
    public void Renders_HxPut()
    {
        var helper = CreateHelper();
        helper.HxPut = "/api/update";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hx-put", "/api/update");
    }

    [Fact]
    public void Renders_HxPatch()
    {
        var helper = CreateHelper();
        helper.HxPatch = "/api/patch";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hx-patch", "/api/patch");
    }

    [Fact]
    public void Renders_HxDelete()
    {
        var helper = CreateHelper();
        helper.HxDelete = "/api/remove";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hx-delete", "/api/remove");
    }

    // ── Behavior attributes ──

    [Fact]
    public void Renders_All_Behavior_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/test";
        helper.HxTarget = "#result";
        helper.HxSwap = "outerHTML";
        helper.HxTrigger = "click";
        helper.HxIndicator = "#spinner";
        helper.HxConfirm = "Are you sure?";
        helper.HxPushUrl = "true";
        helper.HxBoost = "true";
        helper.HxVals = "{\"key\":\"val\"}";
        helper.HxHeaders = "{\"X-Custom\":\"1\"}";
        helper.HxDisabledElt = "this";
        helper.HxEncoding = "multipart/form-data";
        helper.HxExt = "json-enc";
        helper.HxInclude = "[name='token']";
        helper.HxParams = "*";
        helper.HxSelect = "#content";
        helper.HxSelectOob = "#sidebar";
        helper.HxSwapOob = "true";
        helper.HxSync = "closest form:abort";

        var context = CreateContext();
        var output = CreateOutput();
        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/test");
        AssertAttribute(output, "hx-target", "#result");
        AssertAttribute(output, "hx-swap", "outerHTML");
        AssertAttribute(output, "hx-trigger", "click");
        AssertAttribute(output, "hx-indicator", "#spinner");
        AssertAttribute(output, "hx-confirm", "Are you sure?");
        AssertAttribute(output, "hx-push-url", "true");
        AssertAttribute(output, "hx-boost", "true");
        AssertAttribute(output, "hx-vals", "{\"key\":\"val\"}");
        AssertAttribute(output, "hx-headers", "{\"X-Custom\":\"1\"}");
        AssertAttribute(output, "hx-disabled-elt", "this");
        AssertAttribute(output, "hx-encoding", "multipart/form-data");
        AssertAttribute(output, "hx-ext", "json-enc");
        AssertAttribute(output, "hx-include", "[name='token']");
        AssertAttribute(output, "hx-params", "*");
        AssertAttribute(output, "hx-select", "#content");
        AssertAttribute(output, "hx-select-oob", "#sidebar");
        AssertAttribute(output, "hx-swap-oob", "true");
        AssertAttribute(output, "hx-sync", "closest form:abort");
    }

    [Fact]
    public void Null_Attributes_Are_Not_Rendered()
    {
        var helper = CreateHelper();
        // leave everything null
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertNoAttribute(output, "hx-get");
        AssertNoAttribute(output, "hx-post");
        AssertNoAttribute(output, "hx-target");
        AssertNoAttribute(output, "hx-swap");
        AssertNoAttribute(output, "hx-trigger");
        AssertNoAttribute(output, "hx-confirm");
    }

    // ── URL generation ──

    [Fact]
    public void EmptyString_HxGet_Triggers_RouteGeneration_With_Page()
    {
        var helper = CreateHelper("/Items");
        helper.HxGet = "";
        helper.HxPage = "/Items";

        var context = CreateContext();
        var output = CreateOutput();
        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/Items");
    }

    [Fact]
    public void EmptyString_HxPost_Triggers_RouteGeneration_With_Controller_Action()
    {
        var helper = CreateHelper("/Home/Create");
        helper.HxPost = "";
        helper.HxController = "Home";
        helper.HxAction = "Create";

        var context = CreateContext();
        var output = CreateOutput();
        helper.Process(context, output);

        AssertAttribute(output, "hx-post", "/Home/Create");
    }

    [Fact]
    public void EmptyString_HxDelete_With_No_Route_Config_Does_Not_Render()
    {
        var helper = CreateHelper();
        helper.HxDelete = "";
        // no hx-page, hx-controller, hx-action set

        var context = CreateContext();
        var output = CreateOutput();
        helper.Process(context, output);

        AssertNoAttribute(output, "hx-delete");
    }

    [Fact]
    public void Direct_Url_Takes_Precedence_Over_Route_Generation()
    {
        var helper = CreateHelper("/generated");
        helper.HxGet = "/explicit-url";
        helper.HxPage = "/ShouldBeIgnored";

        var context = CreateContext();
        var output = CreateOutput();
        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/explicit-url");
    }

    [Fact]
    public void GenerateRouteUrl_Returns_Null_Without_UrlHelperFactory()
    {
        var helper = new TestTagHelper(); // no factory
        helper.ViewContext = CreateViewContext();
        helper.HxPage = "/Items";

        var result = helper.TestGenerateRouteUrl();
        Assert.Null(result);
    }

    [Fact]
    public void GenerateRouteUrl_Returns_Null_Without_Route_Properties()
    {
        var helper = CreateHelper();
        // no HxPage, HxController, or HxAction

        var result = helper.TestGenerateRouteUrl();
        Assert.Null(result);
    }

    // ── CSS helper methods ──

    [Fact]
    public void BuildCssClass_Filters_Nulls_And_Empties()
    {
        var helper = CreateHelper();
        var result = helper.TestBuildCssClass("a", null, "", "  ", "b");
        Assert.Equal("a b", result);
    }

    [Fact]
    public void ConditionalCssClass_True_ReturnsClass()
    {
        var helper = CreateHelper();
        Assert.Equal("active", helper.TestConditionalCssClass("active", true));
    }

    [Fact]
    public void ConditionalCssClass_False_ReturnsNull()
    {
        var helper = CreateHelper();
        Assert.Null(helper.TestConditionalCssClass("active", false));
    }

    [Fact]
    public void VariantCssClass_WithVariant_ReturnsBemClass()
    {
        var helper = CreateHelper();
        Assert.Equal("rhx-button--brand", helper.TestVariantCssClass("button", "brand"));
    }

    [Fact]
    public void VariantCssClass_NullVariant_ReturnsNull()
    {
        var helper = CreateHelper();
        Assert.Null(helper.TestVariantCssClass("button", null));
    }

    [Fact]
    public void VariantCssClass_EmptyVariant_ReturnsNull()
    {
        var helper = CreateHelper();
        Assert.Null(helper.TestVariantCssClass("button", ""));
    }

    [Fact]
    public void SizeCssClass_WithSize_ReturnsBemClass()
    {
        var helper = CreateHelper();
        Assert.Equal("rhx-button--large", helper.TestSizeCssClass("button", "large"));
    }

    [Fact]
    public void SizeCssClass_NullSize_ReturnsNull()
    {
        var helper = CreateHelper();
        Assert.Null(helper.TestSizeCssClass("button", null));
    }

    // ── Common attributes ──

    [Fact]
    public void Renders_Id_Attribute()
    {
        var helper = CreateHelper();
        helper.Id = "my-element";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "id", "my-element");
    }

    [Fact]
    public void Renders_Hidden_Attribute()
    {
        var helper = CreateHelper();
        helper.Hidden = true;
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        AssertAttribute(output, "hidden", "hidden");
    }

    [Fact]
    public void Custom_CssClass_Is_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-extra";
        var context = CreateContext();
        var output = CreateOutput();

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-test"));
        Assert.True(HasClass(output, "my-extra"));
    }
}
