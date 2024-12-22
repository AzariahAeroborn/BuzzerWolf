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

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            // nextjs static .net hosting integration
            // TODO: nextjsstatichosting docs assumed this would already be present
            //  it might be conflicting with the Server or Swagger?
            //  "System.InvalidOperationException: Endpoint BuzzerWolf.Server.Controllers.CountryController.Index (BuzzerWolf.Server) contains authorization metadata, but a middleware was not found that supports authorization.
            //  Configure your application startup by adding app.UseAuthorization() in the application startup code. If there are calls to app.UseRouting() and app.UseEndpoints(...), the call to app.UseAuthorization() must go between them."
            // I imagine it will eventually be needed for something but the currently very-basic implementation for the frontend is working as intended for now
            // Sounds like there's probably a solution with implementing some auth middleware but it's well beyond my windows server knowledge :)
            //app.UseRouting(); 
            app.MapNextjsStaticHtmls();
            app.UseNextjsStaticHosting();
            
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
