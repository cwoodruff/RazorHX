using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class CarouselTagHelperTests : TagHelperTestBase
{
    private CarouselTagHelper CreateHelper()
    {
        var helper = new CarouselTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    private CarouselItemTagHelper CreateItemHelper()
    {
        var helper = new CarouselItemTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    /// <summary>
    /// Seeds the context with a pre-populated slide count list so the parent
    /// renders navigation/pagination as if N children were processed.
    /// </summary>
    private static void SeedSlideCount(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext context, int count)
    {
        var list = new List<int>();
        for (var i = 0; i < count; i++) list.Add(1);
        context.Items["CarouselSlideCount"] = list;
    }

    // ══════════════════════════════════════════════
    //  CarouselTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-carousel"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-carousel", "");
    }

    [Fact]
    public async Task Has_Region_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "region");
    }

    [Fact]
    public async Task Has_Carousel_Roledescription()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-roledescription", "carousel");
    }

    [Fact]
    public async Task AriaLabel_Rendered_When_Set()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Image gallery";
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Image gallery");
    }

    [Fact]
    public async Task Content_Has_Viewport()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-carousel__viewport", content);
    }

    [Fact]
    public async Task Content_Has_Track_With_AriaLive()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-carousel__track", content);
        Assert.Contains("aria-live=\"polite\"", content);
    }

    // ══════════════════════════════════════════════
    //  CarouselTagHelper — Data attributes
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Slide_Count_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 5);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-slide-count", "5");
    }

    [Fact]
    public async Task Default_Orientation_Horizontal()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 2);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-orientation", "horizontal");
    }

    [Fact]
    public async Task Vertical_Orientation()
    {
        var helper = CreateHelper();
        helper.Orientation = "vertical";
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 2);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-orientation", "vertical");
        Assert.True(HasClass(output, "rhx-carousel--vertical"));
    }

    [Fact]
    public async Task Loop_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Loop = true;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-loop", "true");
    }

    [Fact]
    public async Task No_Loop_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-loop");
    }

    [Fact]
    public async Task Autoplay_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Autoplay = true;
        helper.AutoplayInterval = 3000;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-autoplay", "3000");
    }

    [Fact]
    public async Task No_Autoplay_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-autoplay");
    }

    [Fact]
    public async Task Default_Slides_Per_Page()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-slides-per-page", "1");
    }

    [Fact]
    public async Task Custom_Slides_Per_Page()
    {
        var helper = CreateHelper();
        helper.SlidesPerPage = 3;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 6);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-slides-per-page", "3");
    }

    [Fact]
    public async Task Default_Slides_Per_Move()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-slides-per-move", "1");
    }

    [Fact]
    public async Task Mouse_Dragging_Default_True()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-mouse-dragging", "true");
    }

    [Fact]
    public async Task Mouse_Dragging_Disabled()
    {
        var helper = CreateHelper();
        helper.MouseDragging = false;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-mouse-dragging");
    }

    // ══════════════════════════════════════════════
    //  CarouselTagHelper — Navigation
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Navigation_Renders_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-carousel__navigation", content);
        Assert.Contains("rhx-carousel__nav-button--prev", content);
        Assert.Contains("rhx-carousel__nav-button--next", content);
    }

    [Fact]
    public async Task Navigation_Hidden_When_Disabled()
    {
        var helper = CreateHelper();
        helper.Navigation = false;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-carousel__navigation", content);
    }

    [Fact]
    public async Task Navigation_Hidden_With_Single_Slide()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 1);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-carousel__navigation", content);
    }

    [Fact]
    public async Task Nav_Buttons_Have_Aria_Labels()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Previous slide\"", content);
        Assert.Contains("aria-label=\"Next slide\"", content);
    }

    // ══════════════════════════════════════════════
    //  CarouselTagHelper — Pagination
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Pagination_Renders_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-carousel__pagination", content);
        Assert.Contains("role=\"tablist\"", content);
    }

    [Fact]
    public async Task Pagination_Dot_Count_Matches_Slides()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 4);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Slide 1\"", content);
        Assert.Contains("aria-label=\"Slide 2\"", content);
        Assert.Contains("aria-label=\"Slide 3\"", content);
        Assert.Contains("aria-label=\"Slide 4\"", content);
    }

    [Fact]
    public async Task First_Dot_Selected_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // First dot: aria-selected="true" tabindex="0"
        Assert.Contains("aria-label=\"Slide 1\" aria-selected=\"true\" tabindex=\"0\"", content);
        // Other dots: aria-selected="false" tabindex="-1"
        Assert.Contains("aria-label=\"Slide 2\" aria-selected=\"false\" tabindex=\"-1\"", content);
    }

    [Fact]
    public async Task Pagination_Hidden_When_Disabled()
    {
        var helper = CreateHelper();
        helper.Pagination = false;
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 3);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-carousel__pagination", content);
    }

    [Fact]
    public async Task Pagination_Hidden_With_Single_Slide()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 1);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-carousel__pagination", content);
    }

    // ══════════════════════════════════════════════
    //  CarouselTagHelper — htmx & CSS
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-gallery";
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 2);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-carousel"));
        Assert.True(HasClass(output, "my-gallery"));
    }

    [Fact]
    public async Task Htmx_Attributes_Rendered()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/slides";
        helper.HxTarget = "#carousel-content";
        var context = CreateContext("rhx-carousel");
        SeedSlideCount(context, 2);
        var output = CreateOutput("rhx-carousel");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/slides");
        AssertAttribute(output, "hx-target", "#carousel-content");
    }

    // ══════════════════════════════════════════════
    //  CarouselItemTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Item_Renders_Div()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Item_Has_Element_Class()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-carousel__item"));
    }

    [Fact]
    public void Item_Has_Role_Group()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        AssertAttribute(output, "role", "group");
    }

    [Fact]
    public void Item_Has_Slide_Roledescription()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        AssertAttribute(output, "aria-roledescription", "slide");
    }

    [Fact]
    public void Item_Has_AriaLabel_With_Index()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Slide 1");
    }

    [Fact]
    public void Item_Increments_Index()
    {
        var list = new List<int>();

        // First item
        var helper1 = CreateItemHelper();
        var context1 = CreateContext("rhx-carousel-item");
        context1.Items["CarouselSlideCount"] = list;
        var output1 = CreateOutput("rhx-carousel-item");
        helper1.Process(context1, output1);

        // Second item (same list)
        var helper2 = CreateItemHelper();
        var context2 = CreateContext("rhx-carousel-item");
        context2.Items["CarouselSlideCount"] = list;
        var output2 = CreateOutput("rhx-carousel-item");
        helper2.Process(context2, output2);

        AssertAttribute(output1, "aria-label", "Slide 1");
        AssertAttribute(output1, "data-rhx-slide-index", "1");
        AssertAttribute(output2, "aria-label", "Slide 2");
        AssertAttribute(output2, "data-rhx-slide-index", "2");
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public void Item_Custom_CssClass()
    {
        var helper = CreateItemHelper();
        helper.CssClass = "featured";
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-carousel__item"));
        Assert.True(HasClass(output, "featured"));
    }

    [Fact]
    public void Item_Htmx_Attributes()
    {
        var helper = CreateItemHelper();
        helper.HxGet = "/api/slide-content";
        helper.HxTrigger = "revealed";
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/slide-content");
        AssertAttribute(output, "hx-trigger", "revealed");
    }

    [Fact]
    public void Item_Id_Rendered()
    {
        var helper = CreateItemHelper();
        helper.Id = "slide-1";
        var context = CreateContext("rhx-carousel-item");
        context.Items["CarouselSlideCount"] = new List<int>();
        var output = CreateOutput("rhx-carousel-item");

        helper.Process(context, output);

        AssertAttribute(output, "id", "slide-1");
    }
}
