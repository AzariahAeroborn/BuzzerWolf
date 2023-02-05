using BuzzerWolf.BBAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IBBAPIClient, BBAPIClient>(client =>
{
    client.BaseAddress = new Uri("https://bbapi.buzzerbeater.com");
}).AddHeaderPropagation();
builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add(".ASPXAUTH");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHeaderPropagation();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
