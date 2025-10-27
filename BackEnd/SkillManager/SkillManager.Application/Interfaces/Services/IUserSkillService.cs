using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.Interfaces.Services;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(int userId);
    Task AddSkillAsync(int userId, AddUserSkillDto dto);
    Task UpdateSkillAsync(int userId, UpdateUserSkillsDto dto);
    Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync();
    Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName);
    Task DeleteUserSkillAsync(int userId);
}
