using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerGen;
using MultiDatabase.Data; 
using Microsoft.EntityFrameworkCore.SqlServer;
using TestProject.DbContexts;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


if (environment == "Test" && Environment.GetEnvironmentVariable("USE_IN_MEMORY_DB") == "true")
{
    builder.Services.AddDbContext<EmployeeDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
    builder.Services.AddDbContext<UserTestDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
}
else
{
    // Configure MySQL DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("EmployeePortal"),
        new MySqlServerVersion(new Version(8, 0, 21))));

    builder.Services.AddDbContext<Application2DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerUserPortal")));
}
// Configure SQL Server DbContext


//if (provider == "MySql")
//{
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseMySql(builder.Configuration.GetConnectionString("EmployeePortal"),
//        new MySqlServerVersion(new Version(8, 0, 21))));
//    builder.Services.AddDbContext<Application2DbContext>(options =>
//        options.UseMySql(builder.Configuration.GetConnectionString("UserPortal"),
//        new MySqlServerVersion(new Version(8, 0, 21))));

//    // Configure SQL Server DbContexts
//}
//else if(provider == "SqlServer")
//{
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerEmployeePortal")));

//    builder.Services.AddDbContext<Application2DbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerUserPortal")));
//}
//else
//{
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("InMemoryDb"));
//    builder.Services.AddDbContext<Application2DbContext>(options =>
//    options.UseInMemoryDatabase("InMemoryDb2"));

//}


builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
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
