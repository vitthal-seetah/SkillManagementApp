using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.DTOs.Skill;

namespace SkillManager.Application.Services
{
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
        public async Task<IEnumerable<UserSkillDto>> GetMySkillsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var skills = await _userSkillRepository.GetUserSkillsAsync(userId);
            return skills.Select(MapToDto);
        }

        // -------------------------
        // USER: Add a new skill
        // -------------------------
        public async Task AddSkillAsync(int userId, AddUserSkillDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var existing = await _userSkillRepository.GetByCompositeKeyAsync(userId, dto.SkillId);
            if (existing != null)
                throw new InvalidOperationException("Skill already exists for this user.");

            var userSkill = new UserSkill
            {
                UserId = userId,
                SkillId = dto.SkillId,
                LevelId = dto.LevelId,
            };

            await _userSkillRepository.AddAsync(userSkill);
            await _userSkillRepository.SaveChangesAsync();
        }

        // -------------------------
        // USER: Update a skill
        // -------------------------
        public async Task UpdateSkillAsync(int userId, UpdateUserSkillsDto dto)
        {
            var userSkill = await _userSkillRepository.GetByCompositeKeyAsync(userId, dto.SkillId);
            if (userSkill == null)
                throw new InvalidOperationException("Skill not found for this user.");

            userSkill.LevelId = dto.LevelId;

            await _userSkillRepository.UpdateAsync(userSkill);
            await _userSkillRepository.SaveChangesAsync();
        }

        // -------------------------
        // ADMIN / LEADER: Get all skills
        // -------------------------
        public async Task<IEnumerable<UserSkillDto>> GetAllUserSkillsAsync()
        {
            var skills = await _userSkillRepository.GetAllAsync();
            return skills.Select(MapToDto);
        }

        // -------------------------
        // ADMIN / LEADER: Filter by skill name
        // -------------------------
        public async Task<IEnumerable<UserSkillDto>> FilterBySkillAsync(string skillName)
        {
            var skills = await _userSkillRepository.FilterBySkillAsync(skillName);
            return skills.Select(MapToDto);
        }

        // -------------------------
        // ADMIN: Delete a skill
        // -------------------------
        public async Task DeleteUserSkillAsync(int userId)
        {
            var userSkills = await _userSkillRepository.GetUserSkillsAsync(userId);
            if (!userSkills.Any())
                throw new InvalidOperationException("No skills found for this user.");

            foreach (var skill in userSkills)
            {
                await _userSkillRepository.DeleteAsync(userId, skill.SkillId);
            }

            await _userSkillRepository.SaveChangesAsync();
        }

        // -------------------------
        // Helper: Map UserSkill → DTO
        // -------------------------
        private static UserSkillDto MapToDto(UserSkill us)
        {
            return new UserSkillDto
            {
                UserId = us.UserId,
                SkillId = us.SkillId,
                SkillName = us.Skill?.Label ?? "", // <-- Map to SkillName
                SkillCode = us.Skill?.Code ?? "",
                CategoryName = us.Skill?.Category?.Name ?? "",
                CategoryType = us.Skill?.Category?.CategoryType?.Name.ToString() ?? "",
                LevelId = us.LevelId,
                LevelName = us.Level?.Name ?? "",
            };
        }
    }
}
