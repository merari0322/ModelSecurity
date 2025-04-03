using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de líneas temáticas de experiencias en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ExperienceLineThematicController : ControllerBase
    {
        private readonly ExperienceLineThematicBusiness _ExperienceLineThematicBusiness;
        private readonly ILogger<ExperienceLineThematicController> _logger;

        /// <summary>
        /// Constructor controlador de líneas temáticas de experiencias.
        /// </summary>
        /// <param name="ExperienceLineThematicBusiness">Capa de negocios de líneas temáticas de experiencias.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ExperienceLineThematicController(ExperienceLineThematicBusiness ExperienceLineThematicBusiness, ILogger<ExperienceLineThematicController> logger)
        {
            _ExperienceLineThematicBusiness = ExperienceLineThematicBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las líneas temáticas de experiencias en el sistema.
        /// </summary>
        /// <returns>Lista de líneas temáticas de experiencias</returns>
        /// <response code="200">Lista de líneas temáticas de experiencias</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExperiencieLineThematicData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllExperienceLineThematics()
        {
            try
            {
                var experiences = await _ExperienceLineThematicBusiness.GetAllExperienceLineThematicsAsync();
                return Ok(experiences);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las líneas temáticas de experiencias");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una línea temática de experiencia por su ID.
        /// </summary>
        /// <param name="id">ID de la línea temática de experiencia</param>
        /// <returns>Línea temática de experiencia solicitada</returns>
        /// <response code="200">Retorna la línea temática de experiencia solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Línea temática de experiencia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperiencieLineThematicDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExperienceLineThematicById(int id)
        {
            try
            {
                var experience = await _ExperienceLineThematicBusiness.GetExperienceLineThematicByIdAsync(id);
                return Ok(experience);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Línea temática de experiencia no encontrada con ID: {ExperienceLineThematicId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una línea temática de experiencia en el sistema.
        /// </summary>
        /// <param name="experience">Datos de la línea temática de experiencia a crear</param>
        /// <returns>Línea temática de experiencia creada</returns>
        /// <response code="201">Retorna la línea temática de experiencia creada</response>
        /// <response code="400">Datos de la línea temática de experiencia no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExperiencieLineThematicDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateExperienceLineThematic([FromBody] ExperiencieLineThematicDTO experience)
        {
            try
            {
                var createdExperience = await _ExperienceLineThematicBusiness.CreateExperienceLineThematicAsync(experience);
                return CreatedAtAction(nameof(GetExperienceLineThematicById), new { id = createdExperience.Id }, createdExperience);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear línea temática de experiencia");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear línea temática de experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
