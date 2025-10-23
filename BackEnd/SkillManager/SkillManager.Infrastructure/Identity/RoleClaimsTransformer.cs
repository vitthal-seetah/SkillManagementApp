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
        _logger = logger;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;
        //  identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

        if (identity.HasClaim(c => c.Type == ClaimTypes.Role))
            return principal;

        var name = identity.Name;
        if (string.IsNullOrEmpty(name))
            return principal;

        var parts = name.Split('\\');
        if (parts.Length != 2)
            return principal;

        var domain = parts[0];
        var eid = parts[1];

        var user = await _context
            .Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Domain.ToUpper() == domain.ToUpper() && u.Eid.ToUpper() == eid.ToUpper()
            );

        if (user?.Role != null)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.Name));
        }

        return principal;
    }
}
