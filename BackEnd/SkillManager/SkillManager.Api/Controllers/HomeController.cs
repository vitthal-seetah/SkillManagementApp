using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.DTOs.Level;
using SkillManager.Application.Interfaces.Repositories.m;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _levelService;
        private readonly ILogger<LevelsController> _logger;

        public LevelsController(ILevelService levelService, ILogger<LevelsController> logger)
        {
            _levelService = levelService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelDto>>> GetAllLevels()
        {
            try
            {
                var levels = await _levelService.GetAllLevelsAsync();
                return Ok(levels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all levels");
                return StatusCode(500, "An error occurred while retrieving levels");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LevelDto>> GetLevelById([FromRoute] int id)
        {
            try
            {
                var level = await _levelService.GetLevelByIdAsync(id);
                return Ok(level);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Level with ID {LevelId} not found", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving level with ID {LevelId}", id);
                return StatusCode(500, "An error occurred while retrieving the level");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateLevel([FromBody] CreateLevelDto createDto)
        {
            try
            {
                var result = await _levelService.CreateLevelAsync(createDto);

                if (result)
                {
                    return Ok(new { message = "Level created successfully" });
                }

                return BadRequest("Failed to create level");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating level");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating level");
                return StatusCode(500, "An error occurred while creating the level");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LevelDto>> UpdateLevel(
            [FromRoute] int id,
            [FromBody] UpdateLevelDto updateDto
        )
        {
            try
            {
                var updatedLevel = await _levelService.UpdateLevelAsync(id, updateDto);
                return Ok(updatedLevel);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Level with ID {LevelId} not found for update", id);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error while updating level with ID {LevelId}",
                    id
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating level with ID {LevelId}", id);
                return StatusCode(500, "An error occurred while updating the level");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLevel([FromRoute] int id)
        {
            try
            {
                var result = await _levelService.DeleteLevelAsync(id);

                if (result)
                {
                    return Ok(new { message = "Level deleted successfully" });
                }

                return BadRequest("Failed to delete level");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Level with ID {LevelId} not found for deletion", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting level with ID {LevelId}", id);
                return StatusCode(500, "An error occurred while deleting the level");
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> LevelExists([FromRoute] int id)
        {
            try
            {
                var exists = await _levelService.LevelExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while checking if level with ID {LevelId} exists",
                    id
                );
                return StatusCode(500, "An error occurred while checking level existence");
            }
        }
    }
}
