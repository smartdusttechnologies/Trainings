using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Signal_R;
using Signal_R.Hubs;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
//builder.Services.AddSingleton<IDictionary<string, UserModel>>(opt => new Dictionary<string, UserModel>());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseRouting(); 

app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map SignalR hub
    //endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapHub<UserHub>("/userHub");
   

});


app.Run();



















//public class RiderLocationHub : Hub
//{
//    // Method to send rider's location to all connected clients (user and restaurant)
//    public async Task SendLocation(string riderId, double latitude, double longitude)
//    {
//        // Send the location to specific user and restaurant based on riderId
//        await Clients.User(riderId).SendAsync("ReceiveRiderLocation", latitude, longitude);
//        await Clients.Group("restaurantGroup").SendAsync("ReceiveRiderLocation", latitude, longitude);
//    }

//    // Connect user to a group (user or restaurant)
//    public async Task ConnectToGroup(string groupName)
//    {
//        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
//    }
//}
