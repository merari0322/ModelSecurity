using Business;
using Data;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de instituciones en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InstitutionController : ControllerBase
    {
        private readonly InstitutionBusiness _InstitutionBusiness;
        private readonly ILogger<InstitutionController> _logger;

        /// <summary>
        /// Constructor controlador de instituciones.
        /// </summary>
        /// <param name="InstitutionBusiness">Capa de negocios de instituciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public InstitutionController(InstitutionBusiness InstitutionBusiness, ILogger<InstitutionController> logger)
        {
            _InstitutionBusiness = InstitutionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las instituciones en el sistema.
        /// </summary>
        /// <returns>Lista de instituciones</returns>
        /// <response code="200">Lista de instituciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InstitutionData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInstitutions()
        {
            try
            {
                var institutions = await _InstitutionBusiness.GetAllInstitutionsAsync();
                return Ok(institutions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las instituciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una institución por su ID.
        /// </summary>
        /// <param name="id">ID de la institución</param>
        /// <returns>Institución solicitada</returns>
        /// <response code="200">Retorna la institución solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Institución no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InstitutionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInstitutionById(int id)
        {
            try
            {
                var institution = await _InstitutionBusiness.GetInstitutionByIdAsync(id);
                return Ok(institution);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la institución con ID: {InstitutionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Institución no encontrada con ID: {InstitutionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener institución con ID: {InstitutionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una institución en el sistema.
        /// </summary>
        /// <param name="institution">Datos de la institución a crear</param>
        /// <returns>Institución creada</returns>
        /// <response code="201">Retorna la institución creada</response>
        /// <response code="400">Datos de la institución no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(InstitutionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInstitution([FromBody] InstitutionDTO institution)
        {
            try
            {
                var createdInstitution = await _InstitutionBusiness.CreateInstitutionAsync(institution);
                return CreatedAtAction(nameof(GetInstitutionById), new { id = createdInstitution.Id }, createdInstitution);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear institución");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear institución");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
