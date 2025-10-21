using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Application.Services;
using Fiap.CloudGames.Domain.Interfaces;
using Fiap.CloudGames.Infrastructure.Persistence;
using Fiap.CloudGames.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

#region [Game]

builder.Services.AddScoped<IGameService, GameService>();
//builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
