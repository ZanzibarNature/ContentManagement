using ContentAPI.DAL;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Middleware;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;

namespace ContentAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.Listen(IPAddress.Any, 8080);
            //});

            // Add Services
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

            // Add Repositories
            builder.Services.AddScoped<ILocationRepo<Location>, LocationRepo<Location>>();
            builder.Services.AddScoped<IArticleRepo<Article>, ArticleRepo<Article>>();
            builder.Services.AddScoped<IBlobStorageRepo, BlobStorageRepo>();

            // Add service to convert Strings to Enums
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Add middleware to services
            builder.Services.AddScoped<AuthMiddleware>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Add custom middleware for Security
            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapWhen(
                context =>
                {
                    var routeValues = context.Request.RouteValues;

                    // Define your conditions for excluding middleware here.
                    // For instance, if you want to exclude middleware for a specific action in LocationController:
                    if (routeValues["controller"]?.ToString() == "LocationController" &&
                        routeValues["action"]?.ToString() == "GetByKey")
                    {
                        return false;  // Skip the middleware for this route
                    }

                    return true;  // Continue with middleware for other routes
                },
                builder =>
                {
                    builder.UseMiddleware<AuthMiddleware>();
                }
            );

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
