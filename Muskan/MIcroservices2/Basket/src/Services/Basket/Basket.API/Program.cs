using Basket.API.Midddleware;
using BuildingBlock.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddCarter();
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
builder.Services.AddHttpClient();
builder.Services.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));
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
