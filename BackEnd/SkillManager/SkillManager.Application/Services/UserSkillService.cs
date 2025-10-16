using SkillManager.Application.Abstractions.Repository;
using SkillManager.Application.Abstractions.Services;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Services;

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

    // -------------------------
    // USER: Get own skills
    // -------------------------
    public async Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(string userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        var skills = await _userSkillRepository.GetUserSkillsAsync(userId);

        return skills.Select(us => MapToDto(us));
    }

    // -------------------------
    // USER: Add a skill
    // -------------------------
    public async Task AddSkillAsync(string userId, AddUserSkillDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("User not found.");

        var existing = (await _userSkillRepository.GetUserSkillsAsync(userId)).FirstOrDefault(us =>
            us.SkillId == dto.SkillId
        );

        if (existing != null)
            throw new InvalidOperationException("Skill already added for this user.");

        var userSkill = new UserSkill
        {
            UserId = userId,
            SkillId = dto.SkillId,
            Level = dto.Level,
        };

        await _userSkillRepository.AddAsync(userSkill);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------
    // USER: Update a skill
    // -------------------------
    public async Task UpdateSkillAsync(string userId, UpdateUserSkillsDto dto)
    {
        var userSkill = (await _userSkillRepository.GetUserSkillsAsync(userId)).FirstOrDefault(us =>
            us.SkillId == dto.SkillId
        );

        if (userSkill == null)
            throw new InvalidOperationException("Skill not found for this user.");

        userSkill.Level = dto.Level;

        await _userSkillRepository.UpdateAsync(userSkill);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------
    // LEADER / ADMIN: Get all user skills
    // -------------------------
    public async Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync()
    {
        var skills = await _userSkillRepository.GetAllAsync();
        return skills.Select(us => MapToDto(us));
    }

    // -------------------------
    // LEADER / ADMIN: Filter by skill name
    // -------------------------
    public async Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName)
    {
        var skills = await _userSkillRepository.FilterBySkillAsync(skillName);
        return skills.Select(us => MapToDto(us));
    }

    // -------------------------
    // ADMIN: Delete a skill
    // -------------------------
    public async Task DeleteUserSkillAsync(int id)
    {
        await _userSkillRepository.DeleteAsync(id);
        await _userSkillRepository.SaveChangesAsync();
    }

    // -------------------------
    // Helper: Map UserSkill -> UserSkillDto
    // -------------------------
    private UserSkillDto MapToDto(UserSkill us)
    {
        return new UserSkillDto
        {
            Id = us.Id,
            SkillName = us.Skill?.Name,
            SkillCode = us.Skill?.Code,
            SectionName = us.Skill?.SkillSection?.Name,
            CategoryName = us.Skill?.SkillSection?.Category?.Name,
            Level = us.Level,
        };
    }
}
