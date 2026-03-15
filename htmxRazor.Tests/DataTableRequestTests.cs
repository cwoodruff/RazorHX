using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace htmxRazor.Tests;

public class DataTableRequestTests
{
    private static async Task<DataTableRequest> BindFromQuery(string queryString)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString(queryString);

        var bindingContext = new DefaultModelBindingContext
        {
            ModelMetadata = new EmptyModelMetadataProvider()
                .GetMetadataForType(typeof(DataTableRequest)),
            ActionContext = new Microsoft.AspNetCore.Mvc.ActionContext
            {
                HttpContext = httpContext
            }
        };

        var binder = new DataTableRequestModelBinder();
        await binder.BindModelAsync(bindingContext);

        Assert.True(bindingContext.Result.IsModelSet);
        return (DataTableRequest)bindingContext.Result.Model!;
    }

    [Fact]
    public async Task Defaults_When_No_Params()
    {
        var request = await BindFromQuery("");

        Assert.Null(request.Sort);
        Assert.Null(request.SortDirection);
        Assert.Equal(1, request.Page);
        Assert.Equal(10, request.PageSize);
        Assert.Empty(request.Filters);
    }

    [Fact]
    public async Task Binds_Sort_And_Direction()
    {
        var request = await BindFromQuery("?sort=name&dir=desc");

        Assert.Equal("name", request.Sort);
        Assert.Equal("desc", request.SortDirection);
    }

    [Fact]
    public async Task Binds_Page_And_PageSize()
    {
        var request = await BindFromQuery("?page=3&pageSize=25");

        Assert.Equal(3, request.Page);
        Assert.Equal(25, request.PageSize);
    }

    [Fact]
    public async Task Page_Minimum_Is_1()
    {
        var request = await BindFromQuery("?page=-5");

        Assert.Equal(1, request.Page);
    }

    [Fact]
    public async Task PageSize_Clamped_To_500()
    {
        var request = await BindFromQuery("?pageSize=9999");

        Assert.Equal(500, request.PageSize);
    }

    [Fact]
    public async Task Binds_Filter_Parameters()
    {
        var request = await BindFromQuery("?filter_name=John&filter_email=test");

        Assert.Equal(2, request.Filters.Count);
        Assert.Equal("John", request.Filters["name"]);
        Assert.Equal("test", request.Filters["email"]);
    }

    [Fact]
    public async Task Ignores_Empty_Filters()
    {
        var request = await BindFromQuery("?filter_name=&filter_email=test");

        Assert.Single(request.Filters);
        Assert.Equal("test", request.Filters["email"]);
    }

    [Fact]
    public async Task Full_Binding()
    {
        var request = await BindFromQuery("?sort=price&dir=asc&page=2&pageSize=50&filter_category=Electronics");

        Assert.Equal("price", request.Sort);
        Assert.Equal("asc", request.SortDirection);
        Assert.Equal(2, request.Page);
        Assert.Equal(50, request.PageSize);
        Assert.Equal("Electronics", request.Filters["category"]);
    }

    [Fact]
    public void Provider_Returns_Binder_For_DataTableRequest()
    {
        var provider = new DataTableRequestModelBinderProvider();
        var metadataProvider = new EmptyModelMetadataProvider();
        var metadata = metadataProvider.GetMetadataForType(typeof(DataTableRequest));

        var mockContext = new TestModelBinderProviderContext(metadata);
        var binder = provider.GetBinder(mockContext);

        Assert.NotNull(binder);
        Assert.IsType<DataTableRequestModelBinder>(binder);
    }

    [Fact]
    public void Provider_Returns_Null_For_Other_Types()
    {
        var provider = new DataTableRequestModelBinderProvider();
        var metadataProvider = new EmptyModelMetadataProvider();
        var metadata = metadataProvider.GetMetadataForType(typeof(string));

        var mockContext = new TestModelBinderProviderContext(metadata);
        var binder = provider.GetBinder(mockContext);

        Assert.Null(binder);
    }

    /// <summary>Minimal implementation for testing the provider.</summary>
    private sealed class TestModelBinderProviderContext : ModelBinderProviderContext
    {
        public TestModelBinderProviderContext(ModelMetadata metadata) => Metadata = metadata;
        public override ModelMetadata Metadata { get; }
        public override BindingInfo BindingInfo => new();
        public override IModelMetadataProvider MetadataProvider => new EmptyModelMetadataProvider();
        public override IModelBinder CreateBinder(ModelMetadata metadata) => throw new NotImplementedException();
    }
}
