using BuildingBlock.Messaging.RabbitMQ;
using Logging.API.Consumer;
using Logging.API.Data;
using Logging.API.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;

using NLog.Web;
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);
try
{
     // Add services to the container.

     builder.Services.AddControllers();
     var connectionString = builder.Configuration.GetConnectionString("SqlServerDb");
     builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(connectionString));
     //builder.Logging.ClearProviders();
     builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
     builder.Host.UseNLog();
     builder.Services.AddRabbitMq(builder.Configuration);
     //builder.Services.AddScoped<ILogEntryServices, LogEntryServices>();
     //builder.Services.AddHostedService<LogEntryConsumer>();
     //builder.Services.AddSingleton<ILogEntryServices, LogEntryServices>(); // Singleton
     //builder.Services.AddHostedService<LogEntryConsumer>();
     builder.Services.AddScoped<ILogEntryServices, LogEntryServices>(); // Scoped
     builder.Services.AddSingleton<LogEntryConsumer>(); 
     builder.Services.AddHostedService<LogEntryConsumer>();

     //builder.Services.AddSingleton<IElasticClient>(sp =>
     //{
     //     var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
     //         .DefaultMappingFor<LogEntry>(m => m.IndexName("logs"));

     //     return new ElasticClient(settings);
     //});

     // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
     builder.Services.AddEndpointsApiExplorer();
     builder.Services.AddSwaggerGen();


     var app = builder.Build();

     // Configure the HTTP request pipeline.
     if (app.Environment.IsDevelopment())
     {
          app.UseSwagger();
          app.UseSwaggerUI();
     }
     app.UseMigration().GetAwaiter().GetResult();
     app.UseCors("AllowAll");
     app.UseHttpsRedirection();

     app.UseAuthorization();

     app.MapControllers();

     app.Run();
}
catch (Exception ex)
{
     logger.Error(ex, "Application stopped because of exception");
     throw;
}
finally
{
     NLog.LogManager.Shutdown();
}

