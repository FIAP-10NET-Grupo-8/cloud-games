using Fiap.CloudGames.Api;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Domain.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Email;
using Fiap.CloudGames.Infrastructure.Users.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Application.Services;
using Fiap.CloudGames.Domain.Interfaces;
using Fiap.CloudGames.Infrastructure.Persistence;
using Fiap.CloudGames.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Users.Validators.UserRegisterDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	var xmlFile = Path.ChangeExtension(Assembly.GetEntryAssembly()?.Location, ".xml");
	if (File.Exists(xmlFile)) c.IncludeXmlComments(xmlFile);

	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap.CloudGames API", Version = "v1" });
});

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
