using Business;
using Data;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de personas en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        private readonly PersonBusiness _personBusiness;
        private readonly ILogger<PersonController> _logger;

        /// <summary>
        /// Constructor controlador de personas.
        /// </summary>
        /// <param name="personBusiness">Capa de negocios de personas.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PersonController(PersonBusiness personBusiness, ILogger<PersonController> logger)
        {
            _personBusiness = personBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todas las personas del sistema.
        /// </summary>
        /// <returns>Lista de personas</returns>
        /// <response code="200">Lista de personas</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var persons = await _personBusiness.GetAllPersonsAsync();
                return Ok(persons);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las personas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona</param>
        /// <returns>Persona solicitada</returns>
        /// <response code="200">Retorna la persona solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Persona no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {
                var person = await _personBusiness.GetAllPersonsAsync();
                return Ok(person);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la persona con ID: {PersonId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrada con ID: {PersonId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener la persona con ID: {PersonId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una persona en el sistema.
        /// </summary>
        /// <param name="person">Datos de la persona a crear</param>
        /// <returns>Persona creada</returns>
        /// <response code="201">Retorna la persona creada</response>
        /// <response code="400">Datos de la persona no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO person)
        {
            try
            {
                var createdPerson = await _personBusiness.CreatePersonAsync(person);
                return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear persona");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear persona");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
