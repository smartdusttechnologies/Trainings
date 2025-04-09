using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using Serilog.Formatting.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
Log.Logger = new LoggerConfiguration()
      .Enrich.WithProperty("service", "ProductService")
       .Enrich.FromLogContext()
    .WriteTo.Http("http://localhost:5000",
        queueLimitBytes: null,
        textFormatter: new ElasticsearchJsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
     config.RegisterServicesFromAssembly(assembly);
     config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
     config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHttpClient<LoggingServices>();
builder.Services.AddScoped<ILoggingService, LoggingServices>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServerDb"));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}
await app.UseMigration();
app.MapCarter();
app.UseExceptionHandler(option => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
         ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
//app.UseExceptionHandler(exceptionHandleApp =>
//{
//    exceptionHandleApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null)
//        {
//            return;
//        }
//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace
//        };
//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";
//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});
app.Run();
