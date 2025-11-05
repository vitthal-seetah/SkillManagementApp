using SkillManager.Application.DTOs.Level;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class LevelMapper
{
    public static LevelDto ToLevelDto(this Level level)
    {
        return new LevelDto
        {
            Id = level.LevelId,
            Name = level.Name,
            Points = level.Points,
        };
    }

    public static Level ToLevel(this CreateLevelDto createLevelDto)
    {
        return new Level { Name = createLevelDto.Name, Points = createLevelDto.Points };
    }
}
