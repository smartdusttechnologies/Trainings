using Microsoft.AspNetCore.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
