using ContentAPI.DAL;
using ContentAPI.DAL.Interfaces;
using ContentAPI.Domain;
using ContentAPI.Services;
using ContentAPI.Services.Interfaces;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureWebStorage:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureWebStorage:queue"], preferMsi: true);
});

// Add Services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Add Repositories
builder.Services.AddScoped<ILocationRepo<Location>, LocationRepo<Location>>();
builder.Services.AddScoped<IBlobStorageRepo, BlobStorageRepo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
