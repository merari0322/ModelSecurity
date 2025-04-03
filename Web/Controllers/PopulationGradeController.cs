using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{/// <summary>
 /// Controlador para la gestión de grupos de población en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PopulationGradeController : ControllerBase
    {
        private readonly PopulationGradeBusiness _PopulationGroupBusiness;
        private readonly ILogger<PopulationGradeController> _logger;

        /// <summary>
        /// Constructor controlador de grupos de población.
        /// </summary>
        /// <param name="PopulationGroupBusiness">Capa de negocios de grupos de población.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PopulationGradeController(PopulationGradeBusiness PopulationGroupBusiness, ILogger<PopulationGradeController> logger)
        {
            _PopulationGroupBusiness = PopulationGroupBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los grupos de población en el sistema.
        /// </summary>
        /// <returns>Lista de grupos de población</returns>
        /// <response code="200">Lista de grupos de población</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PopulationGradeData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPopulationGroups()
        {
            try
            {
                var groups = await _PopulationGroupBusiness.GetAllPopulationGradesAsync();
                return Ok(groups);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los grupos de población");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un grupo de población por su ID.
        /// </summary>
        /// <param name="id">ID del grupo de población</param>
        /// <returns>Grupo de población solicitado</returns>
        /// <response code="200">Retorna el grupo de población solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Grupo de población no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PopulationGradeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPopulationGroupById(int id)
        {
            try
            {
                var group = await _PopulationGroupBusiness.GetPopulationGradeByIdAsync(id);
                return Ok(group);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el grupo de población con ID: {PopulationGroupId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Grupo de población no encontrado con ID: {PopulationGroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener grupo de población con ID: {PopulationGroupId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un grupo de población en el sistema.
        /// </summary>
        /// <param name="group">Datos del grupo de población a crear</param>
        /// <returns>Grupo de población creado</returns>
        /// <response code="201">Retorna el grupo de población creado</response>
        /// <response code="400">Datos del grupo de población no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(PopulationGradeDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePopulationGroup([FromBody] PopulationGradeDTO group)
        {
            try
            {
                var createdGroup = await _PopulationGroupBusiness.CreatePopulationGradeAsync(group);
                return CreatedAtAction(nameof(GetPopulationGroupById), new { id = createdGroup.Id }, createdGroup);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear grupo de población");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear grupo de población");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}