using Auth0.AspNetCore.Authentication;
using Shopping.Web.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthHeaderHandler>();


builder.Services.AddRefitClient<ICatalogServices>()
    .ConfigureHttpClient(e =>
    {
         e.BaseAddress = new Uri(builder.Configuration["ApiSetting:GatewayAddress"]);
    }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(e =>
    {
         e.BaseAddress = new Uri(builder.Configuration["ApiSetting:GatewayAddress"]);
    }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(e =>
    {
         e.BaseAddress = new Uri(builder.Configuration["ApiSetting:GatewayAddress"]);
    }).AddHttpMessageHandler<AuthHeaderHandler>();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
     options.Domain = builder.Configuration["Auth0:Domain"];
     options.ClientId = builder.Configuration["Auth0:ClientId"];
     options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
     options.CallbackPath = new PathString("/signin-auth0");
     options.Scope = "openid profile email read:basket";
     //options.Audience = builder.Configuration["Auth0:Audience"];
}).WithAccessToken(options =>
{
     options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddRazorPages(options =>
{
     options.Conventions.AuthorizePage("/Cart");
     options.Conventions.AuthorizePage("/OrderList");
     // 👇 new code
     options.Conventions.AuthorizePage("/Account/Logout");
     options.Conventions.AuthorizePage("/Account/Profile");
     // 👆 new code
});

builder.Services.ConfigureApplicationCookie(options =>
{
     options.Cookie.SameSite = SameSiteMode.None; // Allow cookies on cross-site redirects
     options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures cookies are sent only over HTTPS
     options.Cookie.HttpOnly = true; // Prevents client-side JavaScript from accessing the cookie
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
     app.UseExceptionHandler("/Error");
     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
     app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
