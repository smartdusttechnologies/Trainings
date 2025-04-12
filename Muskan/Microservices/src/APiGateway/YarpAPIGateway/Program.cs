using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(ratelimiter =>
{
    ratelimiter.AddFixedWindowLimiter("fixed", option =>
    {
        option.Window = TimeSpan.FromSeconds(10);
        option.PermitLimit = 1;
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
