using FluentValidation;
using SkillManager.Application.DTOs.User;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserDto> _createValidator;
    private readonly IValidator<UpdateUserDto> _updateValidator;

    public UserService(
        IUserRepository userRepository,
        IValidator<CreateUserDto> createValidator,
        IValidator<UpdateUserDto> updateValidator
    )
    {
        _userRepository = userRepository;

        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // -----------------------------
    // Get all users (mapped to DTO) - UPDATED
    // -----------------------------
    public async Task<IEnumerable<UserDto>> GetAllAsync(User currentUser)
    {
        // All users can only see users in their own project
        var users = await _userRepository.GetByProjectIdAsync(currentUser.ProjectId);

        return users.Select(MapToDto);
    }

    public async Task<IEnumerable<UserDto>> GetAllByTeamAsync(User currentUser)
    {
        // All users can only see users in their own project
        var users = await _userRepository.GetByProjectAndTeamAsync(currentUser);

        return users.Select(MapToDto);
    }

    // -----------------------------
    // Get single user by ID - UPDATED
    // -----------------------------
    public async Task<UserDto?> GetUserByIdAsync(int userId, User currentUser)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return null;

        // All users can only access users in their project
        if (user.ProjectId != currentUser.ProjectId)
        {
            return null;
        }

        return MapToDto(user);
    }

    public async Task<UserDto?> GetByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return null;

        return MapToDto(user);
    }

    // -----------------------------
    // Create new user - UPDATED
    // -----------------------------
    public async Task<(bool Success, string Message, UserDto? CreatedUser)> CreateUserAsync(
        CreateUserDto dto,
        User currentUser
    )
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return (false, validation.Errors.First().ErrorMessage, null);

        // Duplicate UT code check
        var existing = await _userRepository.GetByUtCodeAsync(dto.UtCode);
        if (existing != null)
            return (false, "A user with the same UT Code already exists.", null);

        var userRole = await _userRepository.GetRoleByNameAsync(dto.RoleName);
        if (userRole == null)
            return (false, $"Role '{dto.RoleName}' not found.", null);

        // Validate that the target project matches current user's project
        if (dto.ProjectId != currentUser.ProjectId)
        {
            return (false, "You can only create users within your own project.", null);
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Domain = dto.Domain,
            Eid = dto.Eid,
            UtCode = dto.UtCode,
            RefId = dto.RefId,
            Status = Enum.TryParse<UserStatus>(dto.Status, true, out var status)
                ? status
                : UserStatus.Active,
            DeliveryType = Enum.TryParse<DeliveryType>(dto.DeliveryType, true, out var delivery)
                ? delivery
                : DeliveryType.Onshore,
            RoleId = userRole.RoleId,
            ProjectId = dto.ProjectId, // Use the provided project ID (already validated)
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "User created successfully.", MapToDto(user));
    }

    // -----------------------------
    // Update existing user - UPDATED
    // -----------------------------
    public async Task<(bool Success, string Message, UserDto? UpdatedUser)> UpdateUserAsync(
        UpdateUserDto dto,
        User currentUser
    )
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return (false, validation.Errors.First().ErrorMessage, null);

        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            return (false, "User not found.", null);

        // All users can only edit users in their own project
        if (user.ProjectId != currentUser.ProjectId)
            return (false, "You cannot edit users outside your project.", null);

        // Prevent updating project to a different project
        if (dto.ProjectId.HasValue && dto.ProjectId.Value != currentUser.ProjectId)
        {
            return (false, "You can only assign users to your own project.", null);
        }

        // Prevent UTCode duplicates
        if (!string.IsNullOrWhiteSpace(dto.UtCode) && dto.UtCode != user.UtCode)
        {
            var duplicate = await _userRepository.GetByUtCodeAsync(dto.UtCode);
            if (duplicate != null)
                return (false, "Another user with this UT Code already exists.", null);
        }

        // --- Update fields ---
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.UtCode = dto.UtCode ?? user.UtCode;
        user.RefId = dto.RefId ?? user.RefId;
        user.Domain = dto.Domain ?? user.Domain;
        user.Eid = dto.Eid ?? user.Eid;

        // Update ProjectId (must remain within current user's project)
        if (dto.ProjectId.HasValue)
        {
            user.ProjectId = dto.ProjectId.Value;
        }

        // Update TeamId
        if (dto.TeamId.HasValue)
        {
            user.TeamId = dto.TeamId;
        }

        if (
            !string.IsNullOrWhiteSpace(dto.Status)
            && Enum.TryParse<UserStatus>(dto.Status, true, out var parsedStatus)
        )
            user.Status = parsedStatus;

        if (
            !string.IsNullOrWhiteSpace(dto.DeliveryType)
            && Enum.TryParse<DeliveryType>(dto.DeliveryType, true, out var parsedDelivery)
        )
            user.DeliveryType = parsedDelivery;

        if (!string.IsNullOrWhiteSpace(dto.RoleName))
        {
            var role = await _userRepository.GetRoleByNameAsync(dto.RoleName);
            if (role != null)
                user.RoleId = role.RoleId;
        }

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "User updated successfully.", MapToDto(user));
    }

    // -----------------------------
    // Update user role separately - UPDATED
    // -----------------------------
    public async Task<bool> UpdateUserRoleAsync(int userId, string roleName, User currentUser)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        var role = await _userRepository.GetRoleByNameAsync(roleName);
        if (role == null)
            return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        // All users can only update roles for users in their project
        if (user.ProjectId != currentUser.ProjectId)
            return false;

        if (user.RoleId == role.RoleId)
            return false;

        user.RoleId = role.RoleId;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    // -----------------------------
    // Get User entity by domain + EID (for internal use) - UPDATED
    // -----------------------------
    public async Task<User?> GetUserEntityByDomainAndEidAsync(
        string domain,
        string eid,
        User currentUser
    )
    {
        if (string.IsNullOrWhiteSpace(eid))
            return null;

        var user = await _userRepository.GetByDomainAndEidAsync(domain, eid);

        return user;
    }

    // -----------------------------
    // Get UserDto by domain + EID (for UI / Pages) - UPDATED
    // -----------------------------
    public async Task<UserDto?> GetUserByDomainAndEidAsync(
        string domain,
        string eid,
        User currentUser
    )
    {
        if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(eid))
            return null;

        var user = await _userRepository.GetByDomainAndEidAsync(domain, eid);

        // Only return if user is in the same project
        if (user?.ProjectId != currentUser.ProjectId)
            return null;

        return MapToDto(user);
    }

    // -----------------------------
    // New method: Get users by project ID
    // -----------------------------
    public async Task<IEnumerable<UserDto>> GetUsersByProjectIdAsync(
        int projectId,
        User currentUser
    )
    {
        // Users can only access their own project
        if (projectId != currentUser.ProjectId)
            return Enumerable.Empty<UserDto>();

        var users = await _userRepository.GetByProjectIdAsync(projectId);
        return users.Select(MapToDto);
    }

    public async Task<User?> GetUserEntityByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    private static UserDto MapToDto(User u)
    {
        return new UserDto
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            UtCode = u.UtCode,
            RefId = u.RefId ?? string.Empty,
            RoleName = u.Role?.Name ?? string.Empty,
            Domain = u.Domain,
            Eid = u.Eid,
            Status = u.Status,
            DeliveryType = u.DeliveryType,
            TeamId = u.TeamId,
            ProjectId = u.ProjectId,
            TeamName = u.Team?.TeamName ?? string.Empty,
            ProjectName = u.Project?.ProjectName ?? string.Empty,
        };
    }
}
