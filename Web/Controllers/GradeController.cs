using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de calificaciones en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GradeController : ControllerBase
    {
        private readonly GradeBusiness _GradeBusiness;
        private readonly ILogger<GradeController> _logger;

        /// <summary>
        /// Constructor del controlador de calificaciones.
        /// </summary>
        /// <param name="GradeBusiness">Capa de negocios de calificaciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public GradeController(GradeBusiness GradeBusiness, ILogger<GradeController> logger)
        {
            _GradeBusiness = GradeBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las calificaciones del sistema.
        /// </summary>
        /// <returns>Lista de calificaciones</returns>
        /// <response code="200">Lista de calificaciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GradeData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllGrades()
        {
            try
            {
                var grades = await _GradeBusiness.GetAllGradesAsync();
                return Ok(grades);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las calificaciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una calificación por su ID.
        /// </summary>
        /// <param name="id">ID de la calificación</param>
        /// <returns>Calificación solicitada</returns>
        /// <response code="200">Retorna la calificación solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Calificación no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GradeDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGradeById(int id)
        {
            try
            {
                var grade = await _GradeBusiness.GetGradeByIdAsync(id);
                return Ok(grade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la calificación con ID: {GradeId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Calificación no encontrada con ID: {GradeId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener calificación con ID: {GradeId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una calificación en el sistema.
        /// </summary>
        /// <para name="Grade">Datos de la calificación a crear</para>
        /// <returns>Calificación creada</returns>
        /// <response code="201">Retorna la calificación creada</response>
        /// <response code="400">Datos de la calificación no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(GradeDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateGrade([FromBody] GradeDTO Grade)
        {
            try
            {
                var createdGrade = await _GradeBusiness.CreateGradeAsync(Grade);
                return CreatedAtAction(nameof(GetGradeById), new { id = createdGrade.Id }, createdGrade);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear calificación");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear calificación");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

