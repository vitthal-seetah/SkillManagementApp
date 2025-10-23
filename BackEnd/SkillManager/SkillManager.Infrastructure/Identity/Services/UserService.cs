using SkillManager.Application.Abstractions.Identity;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;
using SkillManager.Infrastructure.Abstractions.Repository;

namespace SkillManager.Infrastructure.Identity.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // -----------------------------
        // Get all users
        // -----------------------------
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        // -----------------------------
        // Get single user by ID
        // -----------------------------
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        // -----------------------------
        // Admin: Update UTCode and RefId
        // -----------------------------
        public async Task<bool> UpdateUserIdentifiersAsync(int userId, string utCode, string refId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.UtCode = utCode;
            user.RefId = refId;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        // -----------------------------
        // Manager: Update personal info and status/delivery
        // -----------------------------
        public async Task<bool> UpdateUserDetailsAsync(
            int userId,
            string firstName,
            string lastName,
            string? status = null,
            string? deliveryType = null
        )
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.FirstName = firstName;
            user.LastName = lastName;

            if (
                !string.IsNullOrEmpty(status)
                && Enum.TryParse(status, true, out UserStatus parsedStatus)
            )
                user.Status = parsedStatus;

            if (
                !string.IsNullOrEmpty(deliveryType)
                && Enum.TryParse(deliveryType, true, out DeliveryType parsedDelivery)
            )
                user.DeliveryType = parsedDelivery;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
