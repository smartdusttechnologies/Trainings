using Basket.API.Midddleware;
using BuildingBlock.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

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
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddHttpClient();
// builder.Services.AddHttpClient<TokenValidator>();
builder.Services.AddScoped<IProducerServices, ProducerServices>();
builder.Services.AddScoped(typeof(ILoggingService<>), typeof(LoggingService<>));
//Exceptional Handling 
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("SqlServerDb"))
    .AddRedis(builder.Configuration.GetConnectionString("Redis"));
//builder.Services.AddHttpClient<TokenValidator>();
// builder.Services.AddHttpClient<TokenValidator>();
builder.Services.AddHttpClient<TokenValidator>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
         var handler = new HttpClientHandler
         {
              ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
         };
         return handler;
    });
//builder.Services.AddHttpClient<TokenValidator>()
//    .ConfigurePrimaryHttpMessageHandler(() =>
//    {
//         var handler = new HttpClientHandler
//         {
//              ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//         };
//         return handler;
//    });

//builder.Services.AddTransient<RabbitMqProducerBase<BasketCheckOutEvents>, BasketCheckOutEventProducer>();
builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowAll", policy =>
     {
          policy.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
     });
});
// Registering AutoMapper (Scan the assembly for profiles)
builder.Services.AddAutoMapper(assembly);
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseDeveloperExceptionPage();
     app.UseSwagger();
     app.UseSwaggerUI();
}
//app.UseMiddleware<TokenValidationMiddlerware>();
app.UseHttpsRedirection();


//app.UseAuthentication();
//app.UseAuthorization();Z

//app.MapControllers();
//app.MapCarter();
app.UseMigration().GetAwaiter().GetResult();
app.UseExceptionHandler(options => { });
// app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
// app.UseMiddleware<TokenValidator>();

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
         ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
