using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using PlaylistManager.Data;
using PlaylistManager.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddAzureKeyVault(new Uri("https://playlistmanagerkv.vault.azure.net/"), new DefaultAzureCredential());

builder.Services.AddDbContext<PlaylistManagerCosmos>(options =>
    options.UseCosmos(
        connectionString: builder.Configuration["cosmosConnectionString"],
        databaseName: builder.Configuration["cosmosDatabase"]
    ));


builder.Services.AddSingleton<Utils>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TrackService>();
builder.Services.AddSingleton<AlbumService>();
builder.Services.AddSingleton<PlaylistService>();
builder.Services.AddSingleton<ArtistService>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddScoped<WatchlistService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var cosmos = scope.ServiceProvider.GetRequiredService<PlaylistManagerCosmos>();
    cosmos.Database.EnsureCreated(); 
}

app.UseCors(policy =>
{
    policy.WithOrigins("https://mango-river-0fddba003.3.azurestaticapps.net/").AllowAnyMethod().AllowAnyHeader();
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
