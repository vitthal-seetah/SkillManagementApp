using SkillManager.Application.DTOs;
using SkillManager.Application.DTOs.Skill;

namespace SkillManager.Application.Abstractions.Services;

public interface IUserSkillService
{
    Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(string userId);

    Task AddSkillAsync(string userId, AddUserSkillDto dto);

    Task UpdateSkillAsync(string userId, UpdateUserSkillsDto dto);

    Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync();

    Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName);

    Task DeleteUserSkillAsync(int userSkillId);
}
