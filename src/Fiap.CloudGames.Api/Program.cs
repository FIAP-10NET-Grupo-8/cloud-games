using Fiap.CloudGames.Api.Middleware;
using Fiap.CloudGames.Application.Games.Services;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Application.Users.Options;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Domain.Games.Repositories;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using Fiap.CloudGames.Domain.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Auth;
using Fiap.CloudGames.Infrastructure.Email;
using Fiap.CloudGames.Infrastructure.Games.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Fiap.CloudGames.Infrastructure.UserGamesLibrary.Repositories;
using Fiap.CloudGames.Infrastructure.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Users.Seeders;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Fiap.CloudGames.Application.Orders.Services;
using Fiap.CloudGames.Domain.Orders.Repositories;
using Fiap.CloudGames.Infrastructure.Orders.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add logging with Serilog
try
{
    var logsPath = Path.Combine(AppContext.BaseDirectory, "Logs");
    if (!Directory.Exists(logsPath)) Directory.CreateDirectory(logsPath);
}
catch { }

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Fiap.CloudGames.Api")
    .WriteTo.Console(new JsonFormatter())
    .CreateBootstrapLogger();

builder.Host.UseSerilog((ctx, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(ctx.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service", "Fiap.CloudGames.Api");
});

builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .Validate(o => 
    { 
        o.Validate();
        return true;
    }, "JwtOptions validation")
    .ValidateOnStart();

builder.Services.AddOptions<AdminUserOptions>()
    .Bind(builder.Configuration.GetSection("AdminUser"))
    .Validate(o =>
    {
        o.Validate();
        return true;
    }, "AdminUserOptions validation")
    .ValidateOnStart();

// Add services to the container.
builder.Services.AddSingleton<JwtService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserSeeder, UserSeeder>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IUserGameLibraryRepository, UserGameLibraryRepository>();

builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Users.Validators.UserRegisterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Games.Validators.CreateGameDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Orders.Validators.CreateOrderDtoValidator>();

builder.Services
    .AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["Jwt:Secret"] ?? string.Empty;
        var key = Encoding.UTF8.GetBytes(secret);

        var validateIssuer = !string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Issuer"]);
        var validateAudience = !string.IsNullOrWhiteSpace(builder.Configuration["Jwt:Audience"]);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = validateIssuer,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = validateAudience,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

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

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), sqlite =>
    {
        sqlite.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
    });
});

var app = builder.Build();

// ---- Migrate + Seed (dev) ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    if (app.Environment.IsDevelopment())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
        await seeder.SeedAsync();
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<StructuredLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

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
