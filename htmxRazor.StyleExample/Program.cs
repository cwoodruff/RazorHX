using htmxRazor.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddhtmxRazor(options =>
{
    options.DefaultTheme = "light";
    options.IncludeHtmxScript = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UsehtmxRazor();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
