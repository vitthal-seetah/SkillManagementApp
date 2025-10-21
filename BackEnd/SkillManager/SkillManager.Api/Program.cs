using AppManagement.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --------------------------
// Register Infrastructure (DbContext, Identity, JWT, Repositories)
// --------------------------
builder.Services.AddInfrastructure(builder.Configuration);

// --------------------------
// Controllers
// --------------------------
builder.Services.AddControllers();

// --------------------------
// CORS
// --------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// --------------------------
// Swagger / OpenAPI + JWT
// --------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "SkillManager API",
            Version = "v1",
            Description = "API for managing skills, users, and roles in SkillManager",
        }
    );

    var jwtScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token here (Bearer <token>)",
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement { { jwtScheme, Array.Empty<string>() } }
    );
});

var app = builder.Build();

// --------------------------
// Apply Migrations & Seed Identity
// --------------------------
await app.Services.SeedIdentityAsync();

// --------------------------
// HTTP Request Pipeline
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("all");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
