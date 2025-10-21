namespace SkillManager.Infrastructure.Abstractions.Identity;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAll();
    Task<ApplicationUser?> GetUserById(string userId);
}
