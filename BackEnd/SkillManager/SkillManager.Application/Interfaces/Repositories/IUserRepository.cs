using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Domain.Entities;
using SkillManager.Domain.Entities.Enums;

namespace SkillManager.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByUtCodeAsync(string utCode);
        Task<User> GetByRefIdAsync(string refId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByStatusAsync(UserStatus status);
        Task<IEnumerable<User>> GetByRoleAsync(int roleId);
        Task<IEnumerable<User>> GetByDeliveryTypeAsync(DeliveryType deliveryType);
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int userId);
        Task<bool> ExistsAsync(int userId);
    }
}
