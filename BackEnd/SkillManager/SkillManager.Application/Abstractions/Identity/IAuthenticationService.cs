namespace SkillManager.Application.Abstractions.Identity;

public interface IAuthenticationService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
}
