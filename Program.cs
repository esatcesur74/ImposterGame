using System.Net.Security;
using ImposterGame.DAL;
using ImposterGame.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ADD THIS for CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000",
                          "http://localhost:8080", "https://localhost:7000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Register DAL (DI)
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

builder.Services.AddSingleton<INetworkService, NetworkService>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5000);//HTTP
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=Game}/{action=New}/{id?}");

app.Run();
