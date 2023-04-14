using AspNet.Security.OAuth.Spotify;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlaylistManager.Data;
using PlaylistManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<PlaylistManagerDb>(options =>
    options.UseSqlServer("Server=tcp:playlistmanager.database.windows.net,1433;Initial Catalog=playlistmanager;Persist Security Info=False;User ID=pmadmin;Password=c9AC$3q*@C5B%l&KEMw&Z9%@Pe6S2F0m;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

builder.Services.AddSingleton<Utils>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TrackService>();
builder.Services.AddSingleton<PlaylistService>();
//builder.Services.AddSingleton<PlaylistManagerDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PlaylistManagerDb>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
