using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SkillManager.Application.DTOs.Skill;
using SkillManager.Application.Interfaces.Repositories;
using SkillManager.Application.Interfaces.Services;
using SkillManager.Infrastructure.Exceptions;

namespace SkillManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly ILogger<SkillsController> _logger;

        public SkillsController(ISkillService skillService, ILogger<SkillsController> logger)
        {
            _skillService = skillService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetAllSkills()
        {
            try
            {
                var skills = await _skillService.GetAllSkillsAsync();
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all skills");
                return StatusCode(500, "An error occurred while retrieving skills");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SkillDto>> GetSkillById([FromRoute] int id)
        {
            try
            {
                var skill = await _skillService.GetSkillByIdAsync(id);
                return Ok(skill);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Skill with ID {SkillId} not found", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving skill with ID {SkillId}", id);
                return StatusCode(500, "An error occurred while retrieving the skill");
            }
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<SkillDto>> GetSkillByCode([FromRoute] string code)
        {
            try
            {
                var skill = await _skillService.GetSkillByCodeAsync(code);
                return Ok(skill);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error while getting skill by code {Code}", code);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Skill with code {Code} not found", code);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while retrieving skill with code {Code}",
                    code
                );
                return StatusCode(500, "An error occurred while retrieving the skill");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateSkill([FromBody] CreateSkillDto createDto)
        {
            try
            {
                var result = await _skillService.CreateSkillAsync(createDto);

                if (result)
                {
                    return Ok(new { message = "Skill created successfully" });
                }

                return BadRequest("Failed to create skill");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating skill");
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error while creating skill");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating skill");
                return StatusCode(500, "An error occurred while creating the skill");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SkillDto>> UpdateSkill(
            [FromRoute] int id,
            [FromBody] UpdateSkillDto updateDto
        )
        {
            try
            {
                var updatedSkill = await _skillService.UpdateSkillAsync(id, updateDto);
                return Ok(updatedSkill);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Skill with ID {SkillId} not found for update", id);
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error while updating skill with ID {SkillId}",
                    id
                );
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(
                    ex,
                    "Application error while updating skill with ID {SkillId}",
                    id
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating skill with ID {SkillId}", id);
                return StatusCode(500, "An error occurred while updating the skill");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSkill([FromRoute] int id)
        {
            try
            {
                var result = await _skillService.DeleteSkillAsync(id);

                if (result)
                {
                    return Ok(new { message = "Skill deleted successfully" });
                }

                return BadRequest("Failed to delete skill");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Skill with ID {SkillId} not found for deletion", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting skill with ID {SkillId}", id);
                return StatusCode(500, "An error occurred while deleting the skill");
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkillsByCategory(
            [FromRoute] int categoryId
        )
        {
            try
            {
                var skills = await _skillService.GetSkillsByCategoryAsync(categoryId);
                return Ok(skills);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error while getting skills for category {CategoryId}",
                    categoryId
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while retrieving skills for category {CategoryId}",
                    categoryId
                );
                return StatusCode(500, "An error occurred while retrieving skills");
            }
        }

        [HttpGet("subcategory/{subCategoryId}")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkillsBySubCategory(
            [FromRoute] int subCategoryId
        )
        {
            try
            {
                var skills = await _skillService.GetSkillsBySubCategoryAsync(subCategoryId);
                return Ok(skills);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error while getting skills for subcategory {SubCategoryId}",
                    subCategoryId
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while retrieving skills for subcategory {SubCategoryId}",
                    subCategoryId
                );
                return StatusCode(500, "An error occurred while retrieving skills");
            }
        }

        [HttpGet("critical")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetCriticalSkills()
        {
            try
            {
                var skills = await _skillService.GetCriticalSkillsAsync();
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving critical skills");
                return StatusCode(500, "An error occurred while retrieving critical skills");
            }
        }

        [HttpGet("project-required")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetProjectRequiredSkills()
        {
            try
            {
                var skills = await _skillService.GetProjectRequiredSkillsAsync();
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving project required skills");
                return StatusCode(
                    500,
                    "An error occurred while retrieving project required skills"
                );
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SkillDto>>> SearchSkills([FromQuery] string q)
        {
            try
            {
                var skills = await _skillService.SearchSkillsAsync(q);
                return Ok(skills);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error while searching skills with term {SearchTerm}",
                    q
                );
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while searching skills with term {SearchTerm}",
                    q
                );
                return StatusCode(500, "An error occurred while searching skills");
            }
        }

        // Additional endpoint to check if skill exists
        [HttpGet("{id}/exists")]
        public async Task<ActionResult<bool>> SkillExists([FromRoute] int id)
        {
            try
            {
                // You might want to add this method to your service interface
                // For now, we'll use GetSkillByIdAsync and catch NotFoundException
                await _skillService.GetSkillByIdAsync(id);
                return Ok(true);
            }
            catch (NotFoundException)
            {
                return Ok(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while checking if skill with ID {SkillId} exists",
                    id
                );
                return StatusCode(500, "An error occurred while checking skill existence");
            }
        }
    }
}
