using RazorHX.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddRazorHX(options =>
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
app.UseRazorHX();
app.UseRouting();
app.MapRazorPages();

app.Run();
