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
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    // -----------------------------
    // Get single user by ID
    // -----------------------------
    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user == null ? null : MapToDto(user);
    }

    // -----------------------------
    // Create new user (with validation + duplicate check)
    // -----------------------------
    public async Task<(bool Success, string Message, UserDto? CreatedUser)> CreateUserAsync(
        CreateUserDto dto
    )
    {
        // ✅ Step 1: Validate request
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return (false, validation.Errors.First().ErrorMessage, null);

        // ✅ Step 2: Prevent duplicate UTCode
        var existing = await _userRepository.GetByUtCodeAsync(dto.UtCode);
        if (existing != null)
            return (false, "A user with the same UT Code already exists.", null);

        // ✅ Step 3: Get Role
        var userRole = await _userRepository.GetRoleByNameAsync(dto.RoleName);
        if (userRole == null)
            return (false, $"Role '{dto.RoleName}' not found.", null);

        // ✅ Step 4: Create User Entity
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Domain = dto.Domain,
            Eid = dto.Eid,
            Status = Enum.TryParse<UserStatus>(dto.Status, true, out var status)
                ? status
                : UserStatus.Active, // default if parsing fails
            DeliveryType = Enum.TryParse<DeliveryType>(dto.DeliveryType, true, out var delivery)
                ? delivery
                : DeliveryType.Onshore, // default if parsing fails
            UtCode = dto.UtCode,
            RefId = dto.RefId,
            RoleId = userRole.RoleId,
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "User created successfully.", MapToDto(user));
    }

    // -----------------------------
    // Update existing user
    // -----------------------------
    public async Task<(bool Success, string Message, UserDto? UpdatedUser)> UpdateUserAsync(
        UpdateUserDto dto
    )
    {
        // ✅ Step 1: Validate request
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return (false, validation.Errors.First().ErrorMessage, null);

        // ✅ Step 2: Find existing user
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            return (false, "User not found.", null);

        // ✅ Step 3: Prevent duplicate UTCode
        if (!string.IsNullOrWhiteSpace(dto.UtCode) && dto.UtCode != user.UtCode)
        {
            var duplicate = await _userRepository.GetByUtCodeAsync(dto.UtCode);
            if (duplicate != null)
                return (false, "Another user with this UT Code already exists.", null);
        }

        // ✅ Step 4: Apply updates
        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;
        user.UtCode = dto.UtCode ?? user.UtCode;
        user.RefId = dto.RefId ?? user.RefId;
        user.Domain = dto.Domain ?? user.Domain;
        user.Eid = dto.Eid ?? user.Eid;
        user.TeamId = dto.TeamId ?? user.TeamId;
        // Parse Enums
        if (
            !string.IsNullOrWhiteSpace(dto.Status)
            && Enum.TryParse<UserStatus>(dto.Status, true, out var parsedStatus)
        )
        {
            user.Status = parsedStatus;
        }

        if (
            !string.IsNullOrWhiteSpace(dto.DeliveryType)
            && Enum.TryParse<DeliveryType>(dto.DeliveryType, true, out var parsedDelivery)
        )
        {
            user.DeliveryType = parsedDelivery;
        }

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
            return false; // no change

        user.RoleId = role.RoleId;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return true;
    }

    // -----------------------------
    // 🔹 MapToDto: Converts Entity → DTO
    // -----------------------------
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
