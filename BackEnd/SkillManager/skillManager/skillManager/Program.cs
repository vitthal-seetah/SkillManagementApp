using System;
using System.Security.Claims;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Application.Services;
using SkillManager.Infrastructure.Identity.AppDbContext;
using SkillManager.Infrastructure.Persistence.Repositories;
using SkillManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Repositories
builder.Services.AddScoped<IUserSkillRepository, UserSkillRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClaimsTransformation, RoleClaimsTransformer>();

// Services
builder.Services.AddScoped<IUserSkillService, UserSkillService>();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "EmployeePolicy",
        policy =>
            policy
                .AddAuthenticationSchemes(NegotiateDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
    );
    options.AddPolicy(
        "ManagerPolicy",
        policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type == ClaimTypes.Role && (c.Value == "Manager" || c.Value == "Admin")
                ) // Admin inherits Manager
            )
    );

    options.AddPolicy(
        "AdminPolicy",
        policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")
            )
    );

    options.AddPolicy(
        "TechLeadPolicy",
        policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type == ClaimTypes.Role
                    && (c.Value == "TechLead" || c.Value == "Manager" || c.Value == "Admin")
                )
            )
    );
});
builder
    .Services.AddHttpClient(
        "MyApi",
        client =>
        {
            client.BaseAddress = new Uri("https://localhost:44366/api/"); // your API URL
        }
    )
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            UseDefaultCredentials = true, // passes Windows Auth credentials
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
