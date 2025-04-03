using Business;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using Data;


namespace Web
{/// <summary>
 /// Controlador para la gestión de experiencias en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ExperienceController : ControllerBase
    {
        private readonly ExperienceBusiness _ExperienceBusiness;
        private readonly ILogger<ExperienceController> _logger;

        /// <summary>
        /// Constructor del controlador de experiencias.
        /// </summary>
        /// <param name="ExperienceBusiness">Capa de negocios de calificaciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ExperienceController(ExperienceBusiness ExperienceBusiness, ILogger<ExperienceController> logger)
        {
            _ExperienceBusiness = ExperienceBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las experiencias del sistema.
        /// </summary>
        /// <returns>Lista de experiencias</returns>
        /// <response code="200">Lista de experiencias</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExperienceData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllExperiences()
        {
            try
            {
                var grades = await _ExperienceBusiness.GetAllExperiencesAsync();
                return Ok(grades);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las experiencias");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una experiencia por su ID.
        /// </summary>
        /// <param name="id">ID de la experiencia</param>
        /// <returns>Experiencia solicitada</returns>
        /// <response code="200">Retorna la experiencia solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Experiencia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperienceDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExperienceById(int id)
        {
            try
            {
                var grade = await _ExperienceBusiness.GetExperienceByIdAsync(id);
                return Ok(grade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la experiencia con ID: {ExperienceId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Experiencia no encontrada con ID: {ExperienceId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener experiencia con ID: {ExperienceId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una experiencia en el sistema.
        /// </summary>
        /// <param name="Grade">Datos de la experiencia a crear</param>
        /// <returns>Experiencia creada</returns>
        /// <response code="201">Retorna la experiencia creada</response>
        /// <response code="400">Datos de la experiencia no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExperienceDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceDTO Grade)
        {
            try
            {
                var createdGrade = await _ExperienceBusiness.CreateExperienceAsync(Grade);
                return CreatedAtAction(nameof(GetExperienceById), new { id = createdGrade.Id }, createdGrade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear experiencia");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
