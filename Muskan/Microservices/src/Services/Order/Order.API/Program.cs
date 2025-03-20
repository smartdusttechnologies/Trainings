using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Add service to the container
builder.Services
    .AddApplicationService()
    .AddInfrastructureService(builder.Configuration)
    .AddAPIService();
var app = builder.Build();

//Configuring the HTTP request pipeline
app.UseAPIService();

if(app.Environment.IsDevelopment())
{
    await app.InitialDatabaseAsync();
}
app.Run();
