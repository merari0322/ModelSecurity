using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de evaluaciones en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EvaluationController : ControllerBase
    {
        private readonly EvaluationBusiness _evaluationBusiness;
        private readonly ILogger<EvaluationController> _logger;

        /// <summary>
        /// Constructor controlador de evaluaciones.
        /// </summary>
        /// <param name="evaluationBusiness">Capa de negocios de evaluaciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public EvaluationController(EvaluationBusiness evaluationBusiness, ILogger<EvaluationController> logger)
        {
            _evaluationBusiness = evaluationBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todas las evaluaciones del sistema.
        /// </summary>
        /// <returns>Lista de evaluaciones</returns>
        /// <response code="200">Lista de evaluaciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EvaluationData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllEvaluations()
        {
            try
            {
                var evaluations = await _evaluationBusiness.GetAllEvaluationsAsync();
                return Ok(evaluations);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las evaluaciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una evaluación por su ID.
        /// </summary>
        /// <param name="id">ID de la evaluación</param>
        /// <returns>Evaluación solicitada</returns>
        /// <response code="200">Retorna la evaluación solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Evaluación no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EvaluationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEvaluationById(int id)
        {
            try
            {
                var evaluation = await _evaluationBusiness.GetEvaluationByIdAsync(id);
                return Ok(evaluation);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la evaluación con ID: {EvaluationId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Evaluación no encontrada con ID: {EvaluationId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener evaluación con ID: {EvaluationId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una evaluación en el sistema.
        /// </summary>
        /// <param name="evaluation">Datos de la evaluación a crear</param>
        /// <returns>Evaluación creada</returns>
        /// <response code="201">Retorna la evaluación creada</response>
        /// <response code="400">Datos de la evaluación no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(EvaluationDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateEvaluation([FromBody] EvaluationDTO evaluation)
        {
            try
            {
                var createdEvaluation = await _evaluationBusiness.CreateEvaluationAsync(evaluation);
                return CreatedAtAction(nameof(GetEvaluationById), new { id = createdEvaluation.Id }, createdEvaluation);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear evaluación");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear evaluación");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
