using SkillManager.Application.DTOs.Team;
using SkillManager.Domain.Entities;

namespace SkillManager.Application.Mappers;

public static class TeamMapper
{
    // Map from Entity to DTO
    public static TeamDto ToDto(Team entity)
    {
        if (entity == null)
            return null;

        return new TeamDto
        {
            TeamId = entity.TeamId,
            TeamName = entity.TeamName,
            TeamDescription = entity.TeamDescription,
            TeamLeadId = entity.TeamLeadId ?? 0, // Handle nullable to non-nullable
            MemberCount = entity.Members?.Count ?? 0, // Calculate from Members collection
        };
    }

    // Map from DTO to Entity (for creating new entities)
    public static Team ToEntity(TeamDto dto)
    {
        if (dto == null)
            return null;

        return new Team
        {
            TeamId = dto.TeamId,
            TeamName = dto.TeamName,
            TeamDescription = dto.TeamDescription,
            TeamLeadId = dto.TeamLeadId == 0 ? null : dto.TeamLeadId, // Handle non-nullable to nullable
            Members = new List<User>(), // Initialize empty collection
        };
        // Note: MemberCount is not set as it's calculated from Members collection
    }

    // Update existing entity from DTO
    public static void ToEntity(TeamDto dto, Team entity)
    {
        if (dto == null || entity == null)
            return;

        entity.TeamName = dto.TeamName;
        entity.TeamDescription = dto.TeamDescription;
        entity.TeamLeadId = dto.TeamLeadId == 0 ? null : dto.TeamLeadId;
        // Note: TeamId and Members collection are not updated
        // MemberCount is calculated, so we don't set it
    }

    // Map collection from Entity to DTO
    public static IEnumerable<TeamDto> ToDto(IEnumerable<Team> entities)
    {
        return entities?.Select(ToDto) ?? Enumerable.Empty<TeamDto>();
    }

    // Map collection from DTO to Entity
    public static IEnumerable<Team> ToEntity(IEnumerable<TeamDto> dtos)
    {
        return dtos?.Select(ToEntity) ?? Enumerable.Empty<Team>();
    }
}
