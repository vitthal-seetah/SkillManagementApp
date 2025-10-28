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

// --------------------
// Windows Authentication
// --------------------
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

// --------------------
// Authorization policies
// --------------------
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
// CORS (optional for intranet scenarios)
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
// Middleware
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

// Claims transformation (optional, if you have RoleClaimsTransformer)
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

// --------------------
// Map Razor Pages
// --------------------
app.MapRazorPages();

app.Run();
