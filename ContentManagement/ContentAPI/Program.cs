using ContentAPI.DAL;
using ContentAPI.Middleware;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Service;
using ContentAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            builder.Services.AddAuthentication(defaultScheme: CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

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

            var app = builder.Build();

            // Add custom middleware for Security
            app.UseMiddleware<AuthMiddleware>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
