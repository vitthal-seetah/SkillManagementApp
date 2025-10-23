using System.Security.Claims;
using AppManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));

    options.AddPolicy("EmployeePolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Employee"));

    options.AddPolicy("ManagerPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Manager"));

    options.AddPolicy("TechLeadPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "TechLead"));
});

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
});

var app = builder.Build();

// --------------------------
// Apply Migrations & Seed Identity
// --------------------------
//await app.Services.SeedIdentityAsync();

// --------------------------
// HTTP Request Pipeline
// --------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
builder.Logging.AddConsole();
app.UseHttpsRedirection();
app.UseCors("all");

app.UseAuthentication();
app.Use(
    async (context, next) =>
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var transformer = context.RequestServices.GetRequiredService<IClaimsTransformation>();
            context.User = await transformer.TransformAsync(context.User);
        }
        await next();
    }
);
app.UseAuthorization();

app.MapControllers();

app.Run();
