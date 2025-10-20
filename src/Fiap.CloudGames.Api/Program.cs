using Fiap.CloudGames.Api;
using Fiap.CloudGames.Application.Users.Services;
using Fiap.CloudGames.Domain.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Auth;
using Fiap.CloudGames.Infrastructure.Email;
using Fiap.CloudGames.Infrastructure.Users.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Bind JwtOptions from configuration and validate (fail fast)
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
	?? throw new InvalidOperationException("Jwt configuration section is missing or invalid.");

// Validate JWT configuration early to fail fast on startup
jwtOptions.Validate();

// Add services to the container.
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddSingleton<JwtService>();

builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IEmailSender, ConsoleEmailSender>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Fiap.CloudGames.Application.Users.Validators.UserRegisterDtoValidator>();

var key = Encoding.UTF8.GetBytes(jwtOptions.Secret);
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidIssuer = jwtOptions.Issuer,
		ValidateAudience = true,
		ValidAudience = jwtOptions.Audience,
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
