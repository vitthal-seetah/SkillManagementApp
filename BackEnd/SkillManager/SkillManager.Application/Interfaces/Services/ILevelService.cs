using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillManager.Application.DTOs.Level;

namespace SkillManager.Application.Interfaces.Repositories.m;

public interface ILevelService
{
    Task<LevelDto> GetLevelByIdAsync(int id);
    Task<IEnumerable<LevelDto>> GetAllLevelsAsync();
    Task<bool> CreateLevelAsync(CreateLevelDto createDto);
    Task<LevelDto> UpdateLevelAsync(int id, UpdateLevelDto updateDto);
    Task<bool> DeleteLevelAsync(int id);
    Task<bool> LevelExistsAsync(int id);
}
