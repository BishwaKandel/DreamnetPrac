using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using System.Net.Http.Headers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllersWithViews().AddViewOptions(options =>
{
    options.HtmlHelperOptions.ClientValidationEnabled = true;
});

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Add DistributedMemoryCache (this is required for session state management)
builder.Services.AddDistributedMemoryCache();

// Add Session services
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Makes the session cookie accessible only to the server
    options.Cookie.IsEssential = true; // Marks session cookie as essential for app functionality
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout
});

// Add Authentication services for cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirect to login page if not authenticated
        options.LogoutPath = "/Auth/Logout"; // Redirect to logout page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiry time
        options.SlidingExpiration = true; // Refresh cookie expiry time as long as user is active
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Register IHttpContextAccessor to access HttpContext
builder.Services.AddHttpContextAccessor();

//builder.Services.AddScoped<IAuthService, AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Now you can use session middleware

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=UserIndex}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=login}/{id?}");

app.Run();
