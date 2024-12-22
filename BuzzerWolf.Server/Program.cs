using BuzzerWolf.BBAPI;
using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// nextjs .net static hosting
using NextjsStaticHosting.AspNetCore;

namespace BuzzerWolf.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0#cors-with-named-policy-and-middleware
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000") // can add more origins like policy.WithOrigins("http://localhost:3000", "http://localhost:3001", ...)
                                            .AllowAnyHeader() // Allow any headers (e.g., Content-Type, Authorization)
                                            .AllowAnyMethod() // Allow any HTTP methods (GET, POST, etc.)
                                            .AllowCredentials(); // looks like we're lining up to support this? though in my experience it's a bit annoying
                                  });
            });

            // Support cookie authentication
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            // Add services to the container.
            builder.Services.AddControllers();
            ConfigureServices(builder.Services);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // nextjs .net static hosting
            builder.Services.Configure<NextjsStaticHostingOptions>(builder.Configuration.GetSection("NextjsStaticHosting"));
            builder.Services.AddNextjsStaticHosting();

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BuzzerWolfContext>();
            db.Database.Migrate();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // nextjs static .net hosting integration
            app.UseRouting(); // TODO: nextjsstatichosting docs assumed this would already be present; so far it seems ok that I added it?
            app.MapNextjsStaticHtmls(); // TODO: order? relative to auth, routing, etc?
            app.UseNextjsStaticHosting(); // TODO: order?

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBBAPIClient, BBAPIClient>();
            services.AddTransient<BuzzerWolfContext>();
            services.AddScoped<IBBDataService, BBDataService>();
        }
    }
}
