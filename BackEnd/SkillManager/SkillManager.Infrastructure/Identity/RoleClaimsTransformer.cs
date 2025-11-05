using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillManager.Infrastructure.Identity.AppDbContext;

public class RoleClaimsTransformer : IClaimsTransformation
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoleClaimsTransformer> _logger;

    public RoleClaimsTransformer(
        ApplicationDbContext context,
        ILogger<RoleClaimsTransformer> logger
    )
    {
        _context = context;
        _logger = logger; //M
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Ensure the identity is a claims identity
        if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            return principal;

        // Prevent duplicate role claims
        if (identity.HasClaim(c => c.Type == ClaimTypes.Role))
            return principal;

        // Extract DOMAIN\EID from Windows identity
        var name = identity.Name;
        if (string.IsNullOrWhiteSpace(name))
            return principal;

        var parts = name.Split('\\', 2);
        if (parts.Length != 2)
            return principal;

        var domain = parts[0].Trim();
        var eid = parts[1].Trim();

        try
        {
            var user = await _context
                .Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Domain.ToUpper() == domain.ToUpper() && u.Eid.ToUpper() == eid.ToUpper()
                );

            if (user == null)
            {
                _logger.LogWarning(
                    "User {Domain}\\{Eid} not found in SkillManager database.",
                    domain,
                    eid
                );
                return principal;
            }

            // Add user ID claim
            identity.AddClaim(new Claim("uid", user.UserId.ToString()));

            // Add role claim from DB
            if (user.Role != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name));
            }
            else
            {
                _logger.LogWarning("User {Domain}\\{Eid} has no role assigned.", domain, eid);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while assigning role claims for {Domain}\\{Eid}.",
                domain,
                eid
            );
        }

        return principal;
    }
}
