using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
//Add service to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplicationService(builder.Configuration)
    .AddInfrastructureService(builder.Configuration)
    .AddAPIService(builder.Configuration);

var app = builder.Build();

//Configuring the HTTP request pipeline
app.UseAPIService();

if(app.Environment.IsDevelopment())
{  
    await app.InitialDatabaseAsync();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();
