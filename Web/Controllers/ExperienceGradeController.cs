using Business;
using Microsoft.AspNetCore.Mvc;
using Entity.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Data;

namespace Web
{/// <summary>
 /// Controlador para la gestión de calificaciones de experiencia en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ExperienceGradeController : ControllerBase
    {
        private readonly ExperienceGradeBusiness _ExperienceGradeBusiness;
        private readonly ILogger<ExperienceGradeController> _logger;

        /// <summary>
        /// Constructor del controlador de calificaciones de experiencia.
        /// </summary>
        /// <param name="ExperienceGradeBusiness">Capa de negocios de calificaciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ExperienceGradeController(ExperienceGradeBusiness ExperienceGradeBusiness, ILogger<ExperienceGradeController> logger)
        {
            _ExperienceGradeBusiness = ExperienceGradeBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las calificaciones de experiencia del sistema.
        /// </summary>
        /// <returns>Lista de calificaciones de experiencia</returns>
        /// <response code="200">Lista de calificaciones de experiencia</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExperienceGradeData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllExperienceGrades()
        {
            try
            {
                var grades = await _ExperienceGradeBusiness.GetAllExperienceGradesAsync();
                return Ok(grades);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las calificaciones de experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una calificación de experiencia por su ID.
        /// </summary>
        /// <param name="id">ID de la calificación de experiencia</param>
        /// <returns>Calificación de experiencia solicitada</returns>
        /// <response code="200">Retorna la calificación de experiencia solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Calificación de experiencia no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperienceGradeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExperienceGradeById(int id)
        {
            try
            {
                var grade = await _ExperienceGradeBusiness.GetExperienceGradeByIdAsync(id);
                return Ok(grade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la calificación de experiencia con ID: {ExperienceGradeId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Calificación de experiencia no encontrada con ID: {ExperienceGradeId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener calificación de experiencia con ID: {ExperienceGradeId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una calificación de experiencia en el sistema.
        /// </summary>
        /// <param name="Grade">Datos de la calificación de experiencia a crear</param>
        /// <returns>Calificación de experiencia creada</returns>
        /// <response code="201">Retorna la calificación de experiencia creada</response>
        /// <response code="400">Datos de la calificación de experiencia no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExperienceGradeDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateExperienceGrade([FromBody] ExperienceGradeDTO Grade)
        {
            try
            {
                var createdGrade = await _ExperienceGradeBusiness.CreateExperienceGradeAsync(Grade);
                return CreatedAtAction(nameof(GetExperienceGradeById), new { id = createdGrade.Id }, createdGrade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear calificación de experiencia");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear calificación de experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
