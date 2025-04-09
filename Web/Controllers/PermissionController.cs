using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Web.Controllers
{/// <summary>
 /// Controlador para la gestion de permisos en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionBusiness _PermissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        /// <summary>
        /// Constructor controlador de permisos.
        /// </summary>
        /// <param name="PermissionBusiness">Capa de negocios de permisos.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PermissionController(PermissionBusiness PermissionBusiness, ILogger<PermissionController> logger)
        {
            _PermissionBusiness = PermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema.
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var permissions = await _PermissionBusiness.GetAllPermissionsAsync();
                return Ok(permissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso por su ID.
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var permission = await _PermissionBusiness.GetPermissionByIdAsync(id);
                return Ok(permission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {PermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {PermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {PermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un permiso en el sistema.
        /// </summary>
        /// <param name="permission">Datos del permiso a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el permiso creado</response>
        /// <response code="400">Datos del permiso no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDTO permission)
        {
            try
            {
                var createdPermission = await _PermissionBusiness.CreatePermissionAsync(permission);
                return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.Id }, createdPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
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