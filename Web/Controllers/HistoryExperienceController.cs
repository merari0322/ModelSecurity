using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de experiencias históricas en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class HistoryExperienceController : ControllerBase
    {
        private readonly HistoryExperienceBusiness _HistoryExperienceBusiness;
        private readonly ILogger<HistoryExperienceController> _logger;

        /// <summary>
        /// Constructor controlador de experiencias históricas.
        /// </summary>
        /// <param name="HistoryExperienceBusiness">Capa de negocios de experiencias históricas.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public HistoryExperienceController(HistoryExperienceBusiness HistoryExperienceBusiness, ILogger<HistoryExperienceController> logger)
        {
            _HistoryExperienceBusiness = HistoryExperienceBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las experiencias históricas en el sistema.
        /// </summary>
        /// <returns>Lista de experiencias históricas</returns>
        /// <response code="200">Lista de experiencias históricas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HistoryExperienceData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllHistoryExperiences()
        {
            try
            {
                var experiences = await _HistoryExperienceBusiness.GetAllHistoryExperiencesAsync();
                return Ok(experiences);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las experiencias históricas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una experiencia histórica por su ID.
        /// </summary>
        /// <param name="id">ID de la experiencia histórica</param>
        /// <returns>Experiencia histórica solicitada</returns>
        /// <response code="200">Retorna la experiencia histórica solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Experiencia histórica no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HistoryExperienceDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetHistoryExperienceById(int id)
        {
            try
            {
                var experience = await _HistoryExperienceBusiness.GetHistoryExperienceByIdAsync(id);
                return Ok(experience);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la experiencia histórica con ID: {HistoryExperienceId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Experiencia histórica no encontrada con ID: {HistoryExperienceId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener experiencia histórica con ID: {HistoryExperienceId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una experiencia histórica en el sistema.
        /// </summary>
        /// <param name="experience">Datos de la experiencia histórica a crear</param>
        /// <returns>Experiencia histórica creada</returns>
        /// <response code="201">Retorna la experiencia histórica creada</response>
        /// <response code="400">Datos de la experiencia histórica no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(HistoryExperienceDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateHistoryExperience([FromBody] HistoryExperienceDTO experience)
        {
            try
            {
                var createdExperience = await _HistoryExperienceBusiness.CreateHistoryExperienceAsync(experience);
                return CreatedAtAction(nameof(GetHistoryExperienceById), new { id = createdExperience.Id }, createdExperience);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear experiencia histórica");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear experiencia histórica");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}
