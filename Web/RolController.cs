using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestion de permisos en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolController : ControllerBase
    {
        private readonly IRolBusiness _rolBusiness;
        private readonly ILogger<RolController> _logger;

        /// <summary>
        /// Constructor controlador de permisos.
        /// </summary>
        /// <param name="RolBusiness">Capa de negocios de permisos.</param>
        /// <param name="logger">Logger para registro de enventos</param>
        public RolController(IRolBusiness rolBusiness, ILogger<RolController> logger)
        {
            _rolBusiness = rolBusiness;
            _logger = logger;
        }
        /// <summary>
        /// Obtienes todos los peermisos del sistema.
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var Rols = await _RolBusiness.GetAllRolesAsync();
                return Ok(Rols);
            }
            catch (IExternalScopeProvider ex)
            {
                _logger.LogError(ex, "Error al obtener los permisos");
                return StatusCode(500), new { message = ex.Message });
            }
        }
        /// <summary>
        /// Obtiene un permiso por su ID.
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retona el permiso solicitado</response>
        /// <response code="400">ID proporcionado no valido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {
                var Rol = await _RolBusiness.GetRolByIdAsync(id);
                if (Rol == null)
                    return Ok(Rol);
            }
            catch (ValidationException ex)
            {
                _Logger.LogWarning(ex "Validacion  fallida para el permiso con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFountException ex)
            {
                _Logger.LogWarning(ex, "Permiso no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Crea un permiso en el sistema.
        /// </summary>
        /// <para name="Rol">Datos del permiso a crear</para>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el permiso creado</response>
        /// <response code="400">Datos del permiso no validos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRol([FromBody] RolDto Rol)
        {
            try
            {
                var createRol = await _RolBusiness.CreateRolAsync(Rol);
                return CreatedAtAction(nameof(GetRolById), new { id = createRol.Id }, createRol);
            }

            catch (ValidationException ex)
            {
                _Logger.LogWarning(ex, "Validacion fallida para crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }

        }
    }
}