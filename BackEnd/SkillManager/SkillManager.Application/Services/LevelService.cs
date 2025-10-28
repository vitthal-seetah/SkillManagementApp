using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SkillManager.Application.DTOs.Level;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Mappers;
using SkillManager.Domain.Entities;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Application.Services
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _levelRepository;

        public LevelService(ILevelRepository levelRepository)
        {
            _levelRepository = levelRepository;
        }

        public async Task<LevelDto> GetLevelByIdAsync(int id)
        {
            var level = await _levelRepository.GetByIdAsync(id);
            if (level == null)
            {
                throw new NotFoundException($"Level with ID {id} not found.");
            }

            return level.ToLevelDto();
        }

        public async Task<IEnumerable<LevelDto>> GetAllLevelsAsync()
        {
            var levels = await _levelRepository.GetAllAsync();
            return levels.Select(l => l.ToLevelDto());
        }

        public async Task<bool> CreateLevelAsync(CreateLevelDto createDto)
        {
            // Validation (if not using FluentValidation in controller)
            if (string.IsNullOrWhiteSpace(createDto.Name))
            {
                throw new ValidationException("Level name is required.");
            }

            var level = createDto.ToLevel();
            var createdLevel = await _levelRepository.AddAsync(level);
            return createdLevel;
        }

        public async Task<LevelDto> UpdateLevelAsync(int id, UpdateLevelDto updateDto)
        {
            var existingLevel = await _levelRepository.GetByIdAsync(id);
            if (existingLevel == null)
            {
                throw new NotFoundException($"Level with ID {id} not found.");
            }
            if (!string.IsNullOrEmpty(updateDto.Name))
            {
                existingLevel.Name = updateDto.Name;
            }
            if (updateDto.Points is not null)
            {
                if (updateDto.Points <= 0)
                {
                    throw new ValidationException("Points must be greater than zero.");
                }
                existingLevel.Points = (int)updateDto.Points;
            }
            // Update properties

            await _levelRepository.UpdateAsync(existingLevel);
            return existingLevel.ToLevelDto();
        }

        public async Task<bool> DeleteLevelAsync(int id)
        {
            var level = await _levelRepository.GetByIdAsync(id);
            if (level is null)
            {
                throw new NotFoundException($"Level with ID {id} not found.");
            }

            var isDeleted = await _levelRepository.DeleteAsync(level);
            return isDeleted;
        }

        public async Task<bool> LevelExistsAsync(int id)
        {
            return await _levelRepository.ExistsAsync(id);
        }
    }
}
