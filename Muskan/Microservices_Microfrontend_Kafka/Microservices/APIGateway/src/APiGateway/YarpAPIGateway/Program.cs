using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container

// 🔹 Load Auth0 Configuration
var auth0Domain = builder.Configuration["Auth0:Domain"];
var auth0Audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .ConfigureHttpClient((context, handler) =>
    {
         handler.AllowAutoRedirect = true;

    })
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(ratelimiter =>
{
     ratelimiter.AddFixedWindowLimiter("fixed", option =>
     {
          option.Window = TimeSpan.FromSeconds(10);
          option.PermitLimit = 1;
     });
});
builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowAll", builder =>
         builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

// 🔹 Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
         options.Authority = $"https://{auth0Domain}/"; // Auth0 Domain
         options.Audience = auth0Audience; // Auth0 Audience
         options.TokenValidationParameters = new TokenValidationParameters
         {
              ValidateIssuer = true,
              ValidateIssuerSigningKey = true,
              ValidAudiences = new[] { auth0Audience },
              ValidIssuer = $"https://{auth0Domain}",
              ValidateAudience = true,
              ValidateLifetime = true,
         };
    });
builder.Services.AddAuthorization(options =>
{
     // options.DefaultPolicy = new AuthorizationPolicyBuilder()
     //.RequireAuthenticatedUser() // ONLY AUTHENTICATION REQUIRED
     //.Build();
     options.AddPolicy("read:basket", policy =>
       policy.RequireAuthenticatedUser());
     //policy.RequireAuthenticatedUser()
     //.RequireClaim("scope", new[] { "openid", "profile", "email", "read:basket" }));
});

//builder.Services.AddAuthorization(options =>
//{
//     //options.AddPolicy("auth_policy", policy => // 🔹 Change "default" to "auth_policy"
//     //    policy.RequireAuthenticatedUser());
//     options.AddPolicy("read:basket", p => p.
//           RequireAuthenticatedUser().
//           RequireClaim("scope", "read:basket")); // Replace RequireScope with RequireClaim
//});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
     app.UseSwagger();
     app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
