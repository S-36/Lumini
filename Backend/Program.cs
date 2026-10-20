using Backend.DB.MongoDB;
using Backend.DB.PostgreSQL;
using Backend.JWT;
using Backend.Middleware;
using Backend.src.User.Interface;
using Backend.src.User.repository;
using Backend.src.User.Services;
using DotNetEnv;
using Lumini.Backend.Bycript;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(options =>
{
    // All endpoints require authentication by default
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Lumini-Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token JWT como: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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

// Load Env variables 
Env.Load();

// MongoDB Settings  ----------------------------------------
builder.Services.Configure<MongoDBSettings>(options =>
{
   options.ConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONECTION") ?? throw new InvalidOperationException("DB_CONECTION is not set"); 
   options.DatabaseName = Environment.GetEnvironmentVariable("MONGODB_NAME") ?? throw new InvalidOperationException("DB_NAME is not set"); 
});

// PostgreSQL Settings ----------------------------------------
builder.Services.Configure<SQLSettings>(options =>
{
   options.ConnectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONECTION") ?? throw new InvalidOperationException("DB_CONECTION is not set"); 
   options.DatabaseName = Environment.GetEnvironmentVariable("POSTGRESQL_NAME") ?? throw new InvalidOperationException("DB_NAME is not set"); 
});

var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONECTION") 
    ?? throw new InvalidOperationException("POSTGRESQL_CONECTION is not set");

builder.Services.AddDbContext<PostgreSQLContext>(options =>
    options.UseNpgsql(connectionString));

// JWT Settings ----------------------------------------
builder.Services.Configure<JwtSettings>(options =>
{
   options.SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY_JWT is not set");
   options.Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new InvalidOperationException("AUDIENCE_JWT is not set");
   options.Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new InvalidOperationException("ISSUER_JWT is not set");
   // If Hours is not set use 1 hour 
   options.ExpirationHours = int.TryParse( Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS"), out var hours) ? hours : 1;
    
});

//JWT Authentification Configuration
builder.Services.AddAuthentication(options =>
{   // Set the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true, // Valida el issuer
        ValidateAudience = true, // Valida la audiencia 
        ValidateLifetime = true, // Valida la expiracion
        ValidateIssuerSigningKey = true, // Valida la firma del token

        // Los valores con los que se valida el token
        ValidIssuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new InvalidOperationException("ISSUER_JWT is not set"),
        ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new InvalidOperationException("AUDIENCE_JWT is not set"),
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? throw new InvalidOperationException("SECRET_KEY_JWT is not set"))),
    
        ClockSkew = TimeSpan.Zero // Elimina el tiempo de tolerancia para la expiracion del token
    };
});

builder.Services.AddCors(options =>
{
    // Politica para Desarollo Permite todo
    options.AddPolicy("DevelomentPolicy", policy =>
    {
        // Add Policy configuration 
        policy.AllowAnyOrigin() // LocalHost Frontend URL
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        // Add Policy configuration 
        policy.WithOrigins("https://yourproductionfrontend.com") // Production Frontend URL
        .WithHeaders("Authorization", "Content-Type") //--> Permite Solo los headers de proteccion y contenido en produccion
        .WithMethods("GET", "POST", "PUT", "DELETE"); //--> Allow only specific methods in production 
    });
});



// ======== Primary Services ===============================
builder.Services.AddSingleton<IMongoContext, MongoDBContext>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IBycriptService, BycriptService>();

// ============= SRC services and repositories ======================== 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// start pipeline
var app = builder.Build();

// Use Middleware first in the pipeline to log all requests and responses in the console 
app.UseMiddleware<ConsoleLogs>();

// Swagger if is Development not Production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Cords - Authentication - Authorization - MapControllers
app.UseHttpsRedirection();
// Usa los CORDS dependiendo del entorno
if(app.Environment.IsDevelopment())
{
    app.UseCors("DevelomentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();