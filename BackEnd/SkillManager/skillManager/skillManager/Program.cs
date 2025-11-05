using System.Security.Claims;
using AppManagement.Application;
using AppManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Application & Infrastructure
// --------------------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// --------------------
// Add Razor Pages
// --------------------
builder.Services.AddRazorPages();
builder.WebHost.UseIISIntegration();

// --------------------
// Register Claims Transformer
// --------------------
builder.Services.AddTransient<IClaimsTransformation, RoleClaimsTransformer>();

// --------------------
// Windows Authentication
// --------------------
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// --------------------
// Authorization policies
// --------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeePolicy", policy => policy.RequireAuthenticatedUser());

    options.AddPolicy(
        "ManagerPolicy",
        policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type == ClaimTypes.Role && (c.Value == "Manager" || c.Value == "Admin")
                )
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

// --------------------
// CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// --------------------
// Build the app
// --------------------
var app = builder.Build();

// --------------------
// Middleware (REMOVE CUSTOM MIDDLEWARE)
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("all");

app.UseAuthentication();
app.UseAuthorization(); // This will automatically call your claims transformer

// --------------------
// Map Razor Pages
// --------------------
app.MapRazorPages();

app.Run();
