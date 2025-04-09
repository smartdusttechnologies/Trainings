using Basket.API.Midddleware;
using BuildingBlock.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);


//Log.Logger = new LoggerConfiguration()
//            .MinimumLevel.Debug() // Set the minimum log level
//            .WriteTo.Console() // Log to console
//                               .WriteTo.Http("http://localhost:5006/api/logs") // URL of the logging service
//.CreateLogger();
//Log.Logger = new LoggerConfiguration()
//            .MinimumLevel.Debug() // Set the minimum log level
//            .WriteTo.Console() // Log to console
//            .WriteTo.Http("http://localhost:5006/api/logs",
//                           queueLimitBytes: null, // Optional: Set to null for default
//                           batchSizeLimitBytes: 100, // Optional: Number of logs to send in a batch
//                           period: TimeSpan.FromSeconds(5), // Optional: Time period to wait before sending logs
//                           textFormatter: null // Optional: Custom text formatter
//                                               //logEventLimitBytes: Serilog.Events.LogEventLevel.Information
//                           ) // Optional: Minimum log level for this sink
//            .CreateLogger();
// Add services to the container.
//Application Services
//Add Carter 
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//           .AddJwtBearer(options =>
//           {
//               options.Authority = "https://dev-h2hafjnbquckxeji.us.auth0.com/";
//               options.Audience = "https://localhost:5056";
//           });
//builder.Services.AddAuthorization();

builder.Services.AddCarter();
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
     config.RegisterServicesFromAssembly(assembly);
     config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
     config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
//Data Services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository , provider.GetRequiredService<IDistributedCache>());
//});
//Scrutor Library
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

//Cache 
builder.Services.AddStackExchangeRedisCache(options =>
{
     options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//Grpc Service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
     options.Address = new Uri(builder.Configuration["GrpcSetting:DiscountUrl"]);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
     var handler = new HttpClientHandler
     {
          ServerCertificateCustomValidationCallback =
         HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
     };

     return handler;
});

//Async Service RabbiMQ
builder.Services.AddMessageBroker(builder.Configuration);
//Exceptional Handling
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServerDb"))
    .AddRedis(builder.Configuration.GetConnectionString("Redis"));
//builder.Services.AddHttpClient<TokenValidator>();
builder.Services.AddHttpClient<TokenValidator>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
         var handler = new HttpClientHandler
         {
              ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
         };
         return handler;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}
//app.UseMiddleware<TokenValidationMiddlerware>();
//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();
app.MapCarter();
app.UseMigration().GetAwaiter().GetResult();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
         ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
