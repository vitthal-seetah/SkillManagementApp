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
    // Get all users (mapped to DTO)
    // -----------------------------
    public async Task<IEnumerable<UserDto>> GetAllAsync(User currentUser)
    {
        var users = await _userRepository.GetAllAsync();

        // Admins and Managers can only see users in their project
        if (currentUser.Role?.Name is "Admin" or "Manager")
            users = users.Where(u => u.ProjectId == currentUser.ProjectId).ToList();

        return users.Select(MapToDto);
    }

    // -----------------------------
    // Get single user by ID
    // -----------------------------
    public async Task<UserDto?> GetUserByIdAsync(int userId, User currentUser)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return null;

        // Admins and Managers can only access users in their project
        if (
            (currentUser.Role.Name == "Admin" || currentUser.Role.Name == "Manager")
            && user.ProjectId != currentUser.ProjectId
        )
        {
            return null;
        }

        return MapToDto(user);
    }

    // -----------------------------
    // Create new user
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
            ProjectId =
                (currentUser.Role?.Name is "Admin" or "Manager")
                    ? currentUser.ProjectId
                    : dto.ProjectId,
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "User created successfully.", MapToDto(user));
    }

    // -----------------------------
    // Update existing user
    // -----------------------------
    // -----------------------------
    // Update existing user
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

        // Restrict Managers/Admins to their own project
        if (currentUser.Role?.Name is "Admin" or "Manager")
        {
            // If current user has a project ID, check if the target user is in the same project
            if (currentUser.ProjectId.HasValue && user.ProjectId != currentUser.ProjectId)
                return (false, "You cannot edit users outside your project.", null);

            // If current user doesn't have a project ID but target user does, also deny access
            if (!currentUser.ProjectId.HasValue && user.ProjectId.HasValue)
                return (false, "You cannot edit users assigned to projects.", null);
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

        // Update ProjectId (only for Admins and Managers)
        if (currentUser.Role?.Name is "Admin" or "Manager" && dto.ProjectId.HasValue)
        {
            user.ProjectId = dto.ProjectId;
        }

        // Update TeamId (only for Admins and Managers)
        if (currentUser.Role?.Name is "Admin" or "Manager" && dto.TeamId.HasValue)
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
    // Update user role separately
    // -----------------------------
    public async Task<bool> UpdateUserRoleAsync(int userId, string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        var role = await _userRepository.GetRoleByNameAsync(roleName);
        if (role == null)
            return false;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        if (user.RoleId == role.RoleId)
            return false;

        user.RoleId = role.RoleId;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    // -----------------------------
    // Get User entity by domain + EID (for internal use)
    // -----------------------------
    public async Task<User?> GetUserEntityByDomainAndEidAsync(string domain, string eid)
    {
        if (string.IsNullOrWhiteSpace(eid))
            return null;

        var user = await _userRepository.GetByDomainAndEidAsync(domain, eid);
        return user;
    }

    // -----------------------------
    // Get UserDto by domain + EID (for UI / Pages)
    // -----------------------------
    public async Task<UserDto?> GetUserByDomainAndEidAsync(string domain, string eid)
    {
        if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(eid))
            return null;

        var user = await _userRepository.GetByDomainAndEidAsync(domain, eid);
        return user == null ? null : MapToDto(user);
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
            TeamName = u.Team?.TeamName ?? string.Empty,
            TeamId = u.Team?.TeamId ?? 0,
        };
    }
}
