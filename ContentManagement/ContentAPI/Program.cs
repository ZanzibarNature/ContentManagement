using ContentAPI.DAL;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Middleware;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using System.Net;
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

            //if (!builder.Environment.IsDevelopment())
            //{
            //    builder.WebHost.ConfigureKestrel(options =>
            //    {
            //        options.Listen(IPAddress.Any, 8080);
            //    });
            //}

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

            // Middleware pipeline

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Add custom middleware for Security
            app.UseMiddleware<AuthMiddleware>();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
