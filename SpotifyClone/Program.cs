using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpotifyClone.Data;
using SpotifyClone.Services.Implenetation;
using SpotifyClone.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDetailsService, UserDetailsService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IArtistDetailsServices, ArtistDetailsService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// ტოკენის კონფიგურაცია
builder.Services.AddScoped<IJWTService, JWTService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Spotify",
            ValidAudience = "SpotifyUser",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d1c3a98008b83d67cdc372f02a3cd82d52c8f8035a3c1747de848f05690e4e8833fee65e699c56e289bf21ed4488961c9b422b367b1a1e72cbbb88a0299f85ff487909f15a92c7ae7be8e4827322a5041fa124fd710c168b294fbe0bbf3a22660eb58841c1a3a178e6d4ee0b5405aed5ff6881aeedffd97730c976d62d442c59ed4d5bc7227200d9770e0f047d7dbadcc83ab19a1ddfa396e2b239fcde9df77f6a3aca3ea2ecb66abd1b03e40c5c030556c7793c2d746da7a26441c4eb8e1d904b6fac87f095bfec285f4f3bdf8e6d1e63e9514d7129132f910a1481bafa55beec7a1d51269a0d7fd9065686ead9616a2351445c48b9ced5868b8c1d902d15f1")),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("ArtistOnly", policy => policy.RequireRole("Artist"));
    options.AddPolicy("Universal", policy => policy.RequireRole("Owner, Admin, Artist"));
    
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();