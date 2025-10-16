namespace SkillManager.Application.Abstractions.Identity;

public record AuthResponse(string Id, string UserName, string Email, string Token);
