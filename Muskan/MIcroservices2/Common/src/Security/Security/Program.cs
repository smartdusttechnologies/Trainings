using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var auth0Domain = builder.Configuration["Auth0:Domain"];
var auth0Audience = builder.Configuration["Auth0:Audience"];
builder.Services.AddHttpClient();
// Add services to the container.
var config = builder.Configuration;

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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
