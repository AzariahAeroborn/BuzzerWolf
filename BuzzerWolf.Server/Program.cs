using BuzzerWolf.BBAPI;
using BuzzerWolf.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

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

            var app = builder.Build();

            app.UseCookiePolicy(new CookiePolicyOptions {  MinimumSameSitePolicy = SameSiteMode.Strict });

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

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBBAPIClient, BBAPIClient>();
            services.AddTransient<BuzzerWolfContext>();
            services.AddTransient<IBBDataService, BBDataService>();
        }
    }
}
