using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Identity.AppDbContext;

namespace SkillManager.Infrastructure.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly ApplicationDbContext _context;

        public LevelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Level>> GetAllAsync()
        {
            return await _context.Levels.ToListAsync();
        }

        public async Task<Level?> GetByIdAsync(int id)
        {
            return await _context.Levels.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Level level)
        {
            _context.Levels.Update(level);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Level level)
        {
            _context.Levels.Remove(level);
            var fieldsDeleted = await _context.SaveChangesAsync();
            if (fieldsDeleted > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddAsync(Level level)
        {
            _context.Levels.Add(level);
            var created = await _context.SaveChangesAsync();
            if (created > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Levels.AnyAsync(l => l.LevelId == id);
        }
    }
}
