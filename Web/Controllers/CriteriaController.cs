using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de criterios en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CriteriaController : ControllerBase
    {
        private readonly CriteriaBusiness _criteriaBusiness;
        private readonly ILogger<CriteriaController> _logger;

        /// <summary>
        /// Constructor controlador de criterios.
        /// </summary>
        /// <param name="criteriaBusiness">Capa de negocios de criterios.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public CriteriaController(CriteriaBusiness criteriaBusiness, ILogger<CriteriaController> logger)
        {
            _criteriaBusiness = criteriaBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todos los criterios del sistema.
        /// </summary>
        /// <returns>Lista de criterios</returns>
        /// <response code="200">Lista de criterios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CriteriaData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCriteria()
        {
            try
            {
                var criteria = await _criteriaBusiness.GetAllCriteriaAsync();
                return Ok(criteria);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los criterios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un criterio por su ID.
        /// </summary>
        /// <param name="id">ID del criterio</param>
        /// <returns>Criterio solicitado</returns>
        /// <response code="200">Retorna el criterio solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Criterio no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CriteriaDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCriteriaById(int id)
        {
            try
            {
                var criteria = await _criteriaBusiness.GetCriteriaByIdAsync(id);
                return Ok(criteria);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el criterio con ID: {CriteriaId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Criterio no encontrado con ID: {CriteriaId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio con ID: {CriteriaId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un criterio en el sistema.
        /// </summary>
        /// <param name="criteria">Datos del criterio a crear</param>
        /// <returns>Criterio creado</returns>
        /// <response code="201">Retorna el criterio creado</response>
        /// <response code="400">Datos del criterio no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(CriteriaDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCriteria([FromBody] CriteriaDTO criteria)
        {
            try
            {
                var createdCriteria = await _criteriaBusiness.CreateCriteriaAsync(criteria);
                return CreatedAtAction(nameof(GetCriteriaById), new { id = createdCriteria.Id }, createdCriteria);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear criterio");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear criterio");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
