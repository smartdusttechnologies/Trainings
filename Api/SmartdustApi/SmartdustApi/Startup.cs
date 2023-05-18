
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SmartdustApi.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Builder;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Repository;
using SmartdustApi.Services.Interfaces;
using SmartdustApi.Services;
using SmartdustApi.Common;
using SmartdustApi.Repository.Interface;

namespace SmartdustApi
{
    public class Startup
    {
        private string MyAllowSpecificOrigins;
        public static TokenValidationParameters tokenValidationParameters;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = tokenValidationParameters;
         });
            //Services
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      //please put your url from where you are going to request
                                      policy.WithOrigins("http://example.com",
                                                          "http://www.contoso.com");
                                  });
            });
            //Repository DI
            services.AddScoped<IContactRepository, ContactRepository>();
            //Service DI
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            //

            services.AddScoped<Services.Interfaces.ILogger,Infrastructure.Logger>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IRoleService, RoleService>();
            //Authorization Handler Initalization Start
            //Authorization Handler Initalization End
            services.AddScoped<ISecurityParameterService, SecurityParameterService>();


            //Repository
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ILoggerRepository, LoggerRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISecurityParameterRepository, SecurityParameterRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            //
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JWT:Secret"])),

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:ValidIssuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration["JWT:ValidAudience"],

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
            app.UseSession();

            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    //context.Request.Headers.Add("Authorization", "Bearer " + token);
                    context.Request.Headers.Add("Authorization", token);
                }
                await next();
            });
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<SdtAuthenticationMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
