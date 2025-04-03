using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de líneas temáticas en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LineThematicController : ControllerBase
    {
        private readonly LineThematicBusiness _lineThematicBusiness;
        private readonly ILogger<LineThematicController> _logger;

        /// <summary>
        /// Constructor controlador de líneas temáticas.
        /// </summary>
        /// <param name="lineThematicBusiness">Capa de negocios de líneas temáticas.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public LineThematicController(LineThematicBusiness lineThematicBusiness, ILogger<LineThematicController> logger)
        {
            _lineThematicBusiness = lineThematicBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todas las líneas temáticas del sistema.
        /// </summary>
        /// <returns>Lista de líneas temáticas</returns>
        /// <response code="200">Lista de líneas temáticas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LineThematicData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllLineThematics()
        {
            try
            {
                var lineThematics = await _lineThematicBusiness.GetAllLineThematicsAsync();
                return Ok(lineThematics);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las líneas temáticas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una línea temática por su ID.
        /// </summary>
        /// <param name="id">ID de la línea temática</param>
        /// <returns>Línea temática solicitada</returns>
        /// <response code="200">Retorna la línea temática solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Línea temática no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LineThematicDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetLineThematicById(int id)
        {
            try
            {
                var lineThematic = await _lineThematicBusiness.GetLineThematicByIdAsync(id);
                return Ok(lineThematic);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la línea temática con ID: {LineThematicId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Línea temática no encontrada con ID: {LineThematicId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la línea temática con ID: {LineThematicId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una línea temática en el sistema.
        /// </summary>
        /// <param name="lineThematic">Datos de la línea temática a crear</param>
        /// <returns>Línea temática creada</returns>
        /// <response code="201">Retorna la línea temática creada</response>
        /// <response code="400">Datos de la línea temática no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(LineThematicDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateLineThematic([FromBody] LineThematicDTO lineThematic)
        {
            try
            {
                var createdLineThematic = await _lineThematicBusiness.CreateLineThematicAsync(lineThematic);
                return CreatedAtAction(nameof(GetLineThematicById), new { id = createdLineThematic.Id }, createdLineThematic);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear línea temática");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear línea temática");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
