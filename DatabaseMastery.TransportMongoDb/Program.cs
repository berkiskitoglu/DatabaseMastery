using DatabaseMastery.TransportMongoDb.Entities;
using DatabaseMastery.TransportMongoDb.Mapping;
using DatabaseMastery.TransportMongoDb.Repositories;
using DatabaseMastery.TransportMongoDb.Services.SliderServices;
using DatabaseMastery.TransportMongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");

if (File.Exists(envPath))
    foreach (var line in File.ReadAllLines(envPath))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
            builder.Configuration[parts[0].Trim().Replace("__", ":")] = parts[1].Trim();
    }


// Database Settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings")); 

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;

    if (settings == null)
        throw new Exception("DatabaseSettings is NULL - check Azure config");

    return settings;
});


builder.Configuration.AddEnvironmentVariables();


// Database Settings
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings__ConnectionString"));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;

    if (settings == null)
        throw new Exception("DatabaseSettings is NULL - check Azure config");

    return settings;
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(GeneralMapping));

// MVC
builder.Services.AddControllersWithViews();


// 🔥 MongoDB Client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IDatabaseSettings>();

    if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        throw new Exception("Mongo ConnectionString is NULL - check Azure config");

    return new MongoClient(settings.ConnectionString);
});


// Database
builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IDatabaseSettings>();
    var client = sp.GetRequiredService<IMongoClient>();

    if (string.IsNullOrWhiteSpace(settings.DatabaseName))
        throw new Exception("Mongo DatabaseName is NULL - check Azure config");

    return client.GetDatabase(settings.DatabaseName);
});


// Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


// Slider Collection
builder.Services.AddScoped<IMongoCollection<Slider>>(sp =>
{
    var db = sp.GetRequiredService<IMongoDatabase>();
    return db.GetCollection<Slider>("Sliders");
});


// Services
builder.Services.AddScoped<ISliderService, SliderService>();


var app = builder.Build();


// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();