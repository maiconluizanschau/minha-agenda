using System.Text;
using Agenda.Api.Application.Cqrs.Contacts.Queries;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Application.Mapping;
using Agenda.Api.Application.Security;
using Agenda.Api.Application.Validation;
using Agenda.Api.Domain.Interfaces;
using Agenda.Api.Infrastructure.Data;
using Agenda.Api.Infrastructure.Data.Repositories;
using Agenda.Api.Infrastructure.Messaging;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "AllowFrontend";

// EF Core with SQLite
builder.Services.AddDbContext<AgendaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// MediatR (CQRS) - v11 style registration
builder.Services.AddMediatR(typeof(Program), typeof(GetAllContactsQuery));

// Repositories
builder.Services.AddScoped<IContactRepository, ContactRepository>();

// RabbitMQ publisher
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

// JWT Token service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Controllers + FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation();

builder.Services.AddTransient<IValidator<CreateContactDto>, CreateContactDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateContactDto>, UpdateContactDtoValidator>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agenda API",
        Version = "v1"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT Bearer no formato: Bearer {seu token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    });
});

// Authentication JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
// fallback aqui só por segurança, mas appsettings
var secretKey = jwtSettings["SecretKey"] ?? "UltraSecure_Jwt_Key_Change_Me_123456789";

builder.Services
    .AddAuthentication(options =>
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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

// Apply migrations / create database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AgendaDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

// Sem redirecionar pra HTTPS porque o compose expõe apenas HTTP (porta 5000)
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
