using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Abstractions.Repository;
using SkillManager.Infrastructure.Abstractions.Services;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Infrastructure.Services;

public class UserSkillService : IUserSkillService
{
    private readonly IUserSkillRepository _userSkillRepository;
    private readonly IUserRepository _userRepository;

    public UserSkillService(
        IUserSkillRepository userSkillRepository,
        IUserRepository userRepository
    )
    {
        _userSkillRepository = userSkillRepository;
        _userRepository = userRepository;
    }

    // -------------------------------------------------------
    // USER: Get own skills
    // -------------------------------------------------------
    public async Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        var skills = await _userSkillRepository.GetUserSkillsAsync(userId);
        return skills.Select(MapToDto);
    }

    // -------------------------------------------------------
    // USER: Add new skill
    // -------------------------------------------------------
    public async Task AddSkillAsync(string userId, AddUserSkillDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        var existing = (await _userSkillRepository.GetUserSkillsAsync(userId)).FirstOrDefault(us =>
            us.SkillId == dto.SkillId
        );

        if (existing != null)
            throw new InvalidOperationException("Skill already exists for this user.");

        var userSkill = new UserSkill
        {
            UserId = 4,
            SkillId = dto.SkillId,
            LevelId = dto.LevelId,
        };

        await _userSkillRepository.AddAsync(userSkill);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------------------------------------
    // USER: Update skill level
    // -------------------------------------------------------
    public async Task UpdateSkillAsync(string userId, UpdateUserSkillsDto dto)
    {
        var userSkills = await _userSkillRepository.GetUserSkillsAsync(userId);
        var userSkill = userSkills.FirstOrDefault(us => us.SkillId == dto.SkillId);

        if (userSkill == null)
            throw new InvalidOperationException("Skill not found for this user.");

        userSkill.LevelId = dto.LevelId;
        await _userSkillRepository.UpdateAsync(userSkill);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------------------------------------
    // ADMIN / LEADER: Get all user skills
    // -------------------------------------------------------
    public async Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync()
    {
        var skills = await _userSkillRepository.GetAllAsync();
        return skills.Select(MapToDto);
    }

    // -------------------------------------------------------
    // ADMIN / LEADER: Filter by skill name
    // -------------------------------------------------------
    public async Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName)
    {
        var skills = await _userSkillRepository.FilterBySkillAsync(skillName);
        return skills.Select(MapToDto);
    }

    // -------------------------------------------------------
    // ADMIN: Delete user skill
    // -------------------------------------------------------
    public async Task DeleteUserSkillAsync(string userId)
    {
        await _userSkillRepository.DeleteAsync(userId);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------------------------------------
    // Helper: Map UserSkill → DTO
    // -------------------------------------------------------
    private static UserSkillDto MapToDto(UserSkill us)
    {
        return new UserSkillDto
        {
            UserId = "$",
            SkillId = us.SkillId,
            LevelId = us.LevelId,
            SkillCode = us.Skill?.Code ?? "",
            CategoryName = us.Skill?.Category?.Name ?? "",
            CategoryType = us.Skill?.Category?.CategoryType?.Name.ToString() ?? "",
            LevelName = us.Level?.Name ?? "",
        };
    }
}
