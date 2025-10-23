using Fiap.CloudGames.Api;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Application.Users.Options;
using Fiap.CloudGames.Domain.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Auth;
using Fiap.CloudGames.Infrastructure.Email;
using Fiap.CloudGames.Infrastructure.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Users.Seeders;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add configuration options with validation
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtOptionsBuilder = builder.Services.AddOptions<JwtOptions>()
    .Bind(jwtSection)
    .Validate(options => { try { options.Validate(); return true; } catch { return false; } }, "JwtOptions validation")
    .ValidateOnStart();

var adminSection = builder.Configuration.GetSection("AdminUser");
var adminOptionsBuilder = builder.Services.AddOptions<AdminUserOptions>()
    .Bind(adminSection)
    .Validate(options => { try { options.Validate(); return true; } catch { return false; } }, "AdminUserOptions validation")
    .ValidateOnStart();

// Add services to the container.
builder.Services.AddSingleton<JwtService>();

builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserSeeder, InMemoryUserSeeder>();

builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Users.Validators.UserRegisterDtoValidator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? string.Empty);

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = Path.ChangeExtension(Assembly.GetEntryAssembly()?.Location, ".xml");
    if (File.Exists(xmlFile)) c.IncludeXmlComments(xmlFile);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap.CloudGames API", Version = "v1" });

    // JWT bearer auth in swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seeder: rodar somente em Development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetService<IUserSeeder>();
    if (seeder != null) await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
