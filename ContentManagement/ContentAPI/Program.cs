using ContentAPI.DAL;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Services;
using ContentAPI.Services.Interfaces;
using System.Net;

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
    
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 8080);
            });

            // Add Services
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

            // Add Repositories
            builder.Services.AddScoped<ILocationRepo<Location>, LocationRepo<Location>>();
            builder.Services.AddScoped<IBlobStorageRepo, BlobStorageRepo>();

            // TO DO: Add security via the KeyCloak server

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
