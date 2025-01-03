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

            // TODO: AP: merging the frontend and the api into kinda this one server is making the auth situation pretty confusing
            //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
            CookieSecurePolicy cookieSecurityPolicy = CookieSecurePolicy.Always;
            if(app.Environment.IsDevelopment())
            {
                cookieSecurityPolicy = CookieSecurePolicy.SameAsRequest;
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None, // Allow cross-origin cookies
                Secure = cookieSecurityPolicy
            });


            app.UseDefaultFiles();
            app.UseStaticFiles();

            // nextjs static .net hosting integration
            app.UseRouting(); // TODO: nextjsstatichosting docs assumed this would already be present; so far it seems ok that I added it?
            app.MapNextjsStaticHtmls(); // TODO: order? relative to auth, routing, etc?
            app.UseNextjsStaticHosting(); // TODO: order?

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "BuzzerWolf API V1");
                    c.RoutePrefix = "api/swagger";
                });
            }

            //app.UseHttpsRedirection();

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
