using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de criterios de evaluación en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EvaluationCriteriaController : ControllerBase
    {
        private readonly EvaluationCriteriaBusiness _evaluationCriteriaBusiness;
        private readonly ILogger<EvaluationCriteriaController> _logger;

        /// <summary>
        /// Constructor controlador de criterios de evaluación.
        /// </summary>
        /// <param name="evaluationCriteriaBusiness">Capa de negocios de criterios de evaluación.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public EvaluationCriteriaController(EvaluationCriteriaBusiness evaluationCriteriaBusiness, ILogger<EvaluationCriteriaController> logger)
        {
            _evaluationCriteriaBusiness = evaluationCriteriaBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todos los criterios de evaluación del sistema.
        /// </summary>
        /// <returns>Lista de criterios de evaluación</returns>
        /// <response code="200">Lista de criterios de evaluación</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EvaluationCriteriaData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEvaluationCriteria()
        {
            try
            {
                var criteria = await _evaluationCriteriaBusiness.GetAllEvaluationCriteriaAsync();
                return Ok(criteria);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los criterios de evaluación");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un criterio de evaluación por su ID.
        /// </summary>
        /// <param name="id">ID del criterio de evaluación</param>
        /// <returns>Criterio de evaluación solicitado</returns>
        /// <response code="200">Retorna el criterio de evaluación solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Criterio de evaluación no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EvaluationCriteriaDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvaluationCriteriaById(int id)
        {
            try
            {
                var criteria = await _evaluationCriteriaBusiness.GetEvaluationCriteriaByIdAsync(id);
                return Ok(criteria);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el criterio de evaluación con ID: {CriteriaId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Criterio de evaluación no encontrado con ID: {CriteriaId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio de evaluación con ID: {CriteriaId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un criterio de evaluación en el sistema.
        /// </summary>
        /// <param name="criteria">Datos del criterio de evaluación a crear</param>
        /// <returns>Criterio de evaluación creado</returns>
        /// <response code="201">Retorna el criterio de evaluación creado</response>
        /// <response code="400">Datos del criterio de evaluación no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(EvaluationCriteriaDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEvaluationCriteria([FromBody] EvaluationCriteriaDTO criteria)
        {
            try
            {
                var createdCriteria = await _evaluationCriteriaBusiness.CreateEvaluationCriteriaAsync(criteria);
                return CreatedAtAction(nameof(GetEvaluationCriteriaById), new { id = createdCriteria.Id }, createdCriteria);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear criterio de evaluación");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear criterio de evaluación");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
