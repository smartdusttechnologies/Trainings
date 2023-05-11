using SmartdustApi.Infrastructure;
using SmartdustApi.Repository;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Services;
using SmartdustApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Repository DI
builder.Services.AddScoped<IContactRepository, ContactRepository>();
//Service DI
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
