using htmxRazor.Components.DataDisplay;
using Xunit;

namespace htmxRazor.Tests;

public class ColumnTagHelperTests : TagHelperTestBase
{
    [Fact]
    public void Column_Suppresses_Output()
    {
        var helper = new ColumnTagHelper { Field = "name", Header = "Name" };
        var context = CreateContext("rhx-column");
        var columns = new List<ColumnDefinition>();
        context.Items["RhxColumns"] = columns;
        var output = CreateOutput("rhx-column");

        helper.Process(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public void Column_Registers_In_Context()
    {
        var helper = new ColumnTagHelper { Field = "name", Header = "Name" };
        var context = CreateContext("rhx-column");
        var columns = new List<ColumnDefinition>();
        context.Items["RhxColumns"] = columns;
        var output = CreateOutput("rhx-column");

        helper.Process(context, output);

        Assert.Single(columns);
        Assert.Equal("name", columns[0].Field);
        Assert.Equal("Name", columns[0].Header);
    }

    [Fact]
    public void Column_Registers_All_Properties()
    {
        var helper = new ColumnTagHelper
        {
            Field = "price",
            Header = "Price",
            Sortable = true,
            SortDirection = "ASC",
            Filterable = true,
            FilterValue = "100",
            Width = "200px",
            Align = "End"
        };
        var context = CreateContext("rhx-column");
        var columns = new List<ColumnDefinition>();
        context.Items["RhxColumns"] = columns;
        var output = CreateOutput("rhx-column");

        helper.Process(context, output);

        var col = columns[0];
        Assert.Equal("price", col.Field);
        Assert.Equal("Price", col.Header);
        Assert.True(col.Sortable);
        Assert.Equal("asc", col.SortDirection); // lowercased
        Assert.True(col.Filterable);
        Assert.Equal("100", col.FilterValue);
        Assert.Equal("200px", col.Width);
        Assert.Equal("end", col.Align); // lowercased
    }

    [Fact]
    public void Multiple_Columns_Register_In_Order()
    {
        var context = CreateContext("rhx-column");
        var columns = new List<ColumnDefinition>();
        context.Items["RhxColumns"] = columns;

        new ColumnTagHelper { Field = "a", Header = "A" }.Process(context, CreateOutput("rhx-column"));
        new ColumnTagHelper { Field = "b", Header = "B" }.Process(context, CreateOutput("rhx-column"));
        new ColumnTagHelper { Field = "c", Header = "C" }.Process(context, CreateOutput("rhx-column"));

        Assert.Equal(3, columns.Count);
        Assert.Equal("a", columns[0].Field);
        Assert.Equal("b", columns[1].Field);
        Assert.Equal("c", columns[2].Field);
    }
}
