using Business;
using Data;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{/// <summary>
 /// Controlador para la gestion de objetivos en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ObjectiveController : ControllerBase
    {
        private readonly ObjectiveBusiness _ObjectiveBusiness;
        private readonly ILogger<ObjectiveController> _logger;

        /// <summary>
        /// Constructor controlador de objetivos.
        /// </summary>
        /// <param name="ObjectiveBusiness">Capa de negocios de objetivos.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ObjectiveController(ObjectiveBusiness ObjectiveBusiness, ILogger<ObjectiveController> logger)
        {
            _ObjectiveBusiness = ObjectiveBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los objetivos del sistema.
        /// </summary>
        /// <returns>Lista de objetivos</returns>
        /// <response code="200">Lista de objetivos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ObjectiveData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllObjectives()
        {
            try
            {
                var objectives = await _ObjectiveBusiness.GetAllObjectivesAsync();
                return Ok(objectives);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los objetivos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un objetivo por su ID.
        /// </summary>
        /// <param name="id">ID del objetivo</param>
        /// <returns>Objetivo solicitado</returns>
        /// <response code="200">Retorna el objetivo solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Objetivo no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ObjectiveDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetObjectiveById(int id)
        {
            try
            {
                var objective = await _ObjectiveBusiness.GetObjectiveByIdAsync(id);
                return Ok(objective);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el objetivo con ID: {ObjectiveId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Objetivo no encontrado con ID: {ObjectiveId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener objetivo con ID: {ObjectiveId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un objetivo en el sistema.
        /// </summary>
        /// <param name="objective">Datos del objetivo a crear</param>
        /// <returns>Objetivo creado</returns>
        /// <response code="201">Retorna el objetivo creado</response>
        /// <response code="400">Datos del objetivo no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ObjectiveDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateObjective([FromBody] ObjectiveDTO objective)
        {
            try
            {
                var createdObjective = await _ObjectiveBusiness.CreateObjectiveAsync(objective);
                return CreatedAtAction(nameof(GetObjectiveById), new { id = createdObjective.Id }, createdObjective);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear objetivo");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear objetivo");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
