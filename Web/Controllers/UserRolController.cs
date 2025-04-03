using Business;
using Data;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de roles de usuario en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserRolController : ControllerBase
    {
        private readonly UserRolBusiness _UserRolBusiness;
        private readonly ILogger<UserRolController> _logger;

        /// <summary>
        /// Constructor controlador de roles de usuario.
        /// </summary>
        /// <param name="UserRolBusiness">Capa de negocios de roles de usuario.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public UserRolController(UserRolBusiness UserRolBusiness, ILogger<UserRolController> logger)
        {
            _UserRolBusiness = UserRolBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles de usuario en el sistema.
        /// </summary>
        /// <returns>Lista de roles de usuario</returns>
        /// <response code="200">Lista de roles de usuario</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserRolData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserRols()
        {
            try
            {
                var roles = await _UserRolBusiness.GetAllUserRolesAsync();
                return Ok(roles);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los roles de usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un rol de usuario por su ID.
        /// </summary>
        /// <param name="id">ID del rol de usuario</param>
        /// <returns>Rol de usuario solicitado</returns>
        /// <response code="200">Retorna el rol de usuario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Rol de usuario no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserRolDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserRolById(int id)
        {
            try
            {
                var rol = await _UserRolBusiness.GetUserRolByIdAsync(id);
                return Ok(rol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el rol de usuario con ID: {UserRolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol de usuario no encontrado con ID: {UserRolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener rol de usuario con ID: {UserRolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un rol de usuario en el sistema.
        /// </summary>
        /// <param name="rol">Datos del rol de usuario a crear</param>
        /// <returns>Rol de usuario creado</returns>
        /// <response code="201">Retorna el rol de usuario creado</response>
        /// <response code="400">Datos del rol de usuario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserRolDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUserRol([FromBody] UserRolDTO rol)
        {
            try
            {
                var createdRol = await _UserRolBusiness.CreateUserRolAsync(rol);
                return CreatedAtAction(nameof(GetUserRolById), new { id = createdRol.Id }, createdRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear rol de usuario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear rol de usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
