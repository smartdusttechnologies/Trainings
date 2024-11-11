using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using MultiDatabase.Data; 
using Microsoft.EntityFrameworkCore.SqlServer;
//using TestProject.DbContexts;
using MultiDatabase.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using MultiDatabase.Repository;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
void ConfigureDbContexts(IServiceCollection services, IConfiguration configuration)
{
    ConfigureDbContext<ApplicationDbContext>(services, configuration, "ApplicationDbContext");
    ConfigureDbContext<Application2DbContext>(services, configuration, "Application2DbContext");
}
//service
void ConfigureDbContext<TContext>(IServiceCollection services, IConfiguration configuration, string dbContextKey) where TContext : DbContext
{
    var dbType = configuration[$"DbContextSettings:{dbContextKey}:Type"];
    var connectionString = configuration.GetConnectionString(dbType == "MySql" ? "EmployeePortal" : "SqlServerUserPortal");

    services.AddDbContext<TContext>(options =>
    {
        switch (dbType)
        {
            case "InMemory":
                options.UseInMemoryDatabase("InMemoryDb"); 
                break;
            case "SqlServer":
                options.UseSqlServer(connectionString);
                break;
            case "MySql":
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
                break;
            default:
                throw new NotSupportedException($"Database type '{dbType}' is not supported.");
        }
    });
}

ConfigureDbContexts(builder.Services, builder.Configuration);

builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
//builder.Services.AddScoped<IDbContextFactory2, DbContextFactory2>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
