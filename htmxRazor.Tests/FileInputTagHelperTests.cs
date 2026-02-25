using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class FileInputTagHelperTests : TagHelperTestBase
{
    private FileInputTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Element rendering ──

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-file-input", "");
    }

    // ── Drop zone ──

    [Fact]
    public void Has_Dropzone()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__dropzone", content);
        Assert.Contains("<label", content);
    }

    [Fact]
    public void Has_Upload_Icon()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__icon", content);
        Assert.Contains("<svg", content);
    }

    [Fact]
    public void Has_Instruction_Text()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__text", content);
        Assert.Contains("browse", content);
    }

    // ── Native file input ──

    [Fact]
    public void Has_Native_File_Input()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"file\"", content);
        Assert.Contains("rhx-file-input__native", content);
    }

    [Fact]
    public void Native_Input_Is_Sr_Only()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-sr-only", content);
    }

    // ── Name and Id ──

    [Fact]
    public void Name_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Name = "avatar";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"avatar\"", content);
    }

    [Fact]
    public void Id_Sets_On_Native_Input()
    {
        var helper = CreateHelper();
        helper.Id = "my-file";
        helper.Name = "avatar";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("id=\"my-file\"", content);
    }

    // ── Accept ──

    [Fact]
    public void Accept_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Accept = "image/*";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("accept=\"image/*\"", content);
    }

    [Fact]
    public void No_Accept_Omits_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("accept=", content);
    }

    // ── Multiple ──

    [Fact]
    public void Multiple_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Multiple = true;
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" multiple", content);
    }

    [Fact]
    public void No_Multiple_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain(" multiple", content);
    }

    // ── Max file size ──

    [Fact]
    public void Max_File_Size_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.MaxFileSize = 5242880;
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-max-size", "5242880");
    }

    [Fact]
    public void No_Max_File_Size_No_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-max-size");
    }

    // ── File list container ──

    [Fact]
    public void Has_File_List_Container()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__file-list", content);
        Assert.Contains("aria-live=\"polite\"", content);
    }

    // ── Label ──

    [Fact]
    public void Label_Renders()
    {
        var helper = CreateHelper();
        helper.Label = "Upload Photo";
        helper.Name = "photo";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__label", content);
        Assert.Contains("Upload Photo", content);
    }

    // ── Hint ──

    [Fact]
    public void Hint_Renders()
    {
        var helper = CreateHelper();
        helper.Hint = "Max 5MB";
        helper.Name = "photo";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-file-input__hint", content);
        Assert.Contains("Max 5MB", content);
    }

    // ── Error ──

    [Fact]
    public void Error_Adds_Modifier_And_Message()
    {
        var helper = CreateHelper();
        helper.Name = "photo";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("photo", "File is required");
        helper.ViewContext = viewContext;
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input--error"));
        var content = output.Content.GetContent();
        Assert.Contains("File is required", content);
        Assert.Contains("aria-invalid=\"true\"", content);
    }

    // ── Disabled ──

    [Fact]
    public void Disabled_Adds_Modifier_And_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    [Fact]
    public void Required_Sets_Attributes()
    {
        var helper = CreateHelper();
        helper.Required = true;
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" required", content);
        Assert.Contains("aria-required=\"true\"", content);
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input--small"));
    }

    [Fact]
    public void Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input--large"));
    }

    // ── htmx ──

    [Fact]
    public void Htmx_On_Native_Input()
    {
        var helper = CreateHelper();
        helper.HxPost = "/upload";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=\"/upload\"", content);
        Assert.Contains("hx-trigger=\"change\"", content);
    }

    [Fact]
    public void Htmx_Auto_Sets_Multipart_Encoding()
    {
        var helper = CreateHelper();
        helper.HxPost = "/upload";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-encoding=\"multipart/form-data\"", content);
    }

    [Fact]
    public void No_Htmx_No_Multipart_Encoding()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("hx-encoding", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-upload";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-file-input"));
        Assert.True(HasClass(output, "my-upload"));
    }

    // ── ARIA label ──

    [Fact]
    public void Aria_Label_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Upload file";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Upload file\"", content);
    }

    // ── Dropzone label for attribute ──

    [Fact]
    public void Dropzone_Label_Has_For_Attribute()
    {
        var helper = CreateHelper();
        helper.Name = "avatar";
        var context = CreateContext("rhx-file-input");
        var output = CreateOutput("rhx-file-input");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("for=\"avatar\"", content);
    }
}
