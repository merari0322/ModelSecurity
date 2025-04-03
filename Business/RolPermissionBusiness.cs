using Data;
using Entity.DTOs;
using Entity.Model;
using Entity.ModelExperience;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;


namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos de roles en el sistema.
    /// </summary>
    public class RolPermissionBusiness
    {
        private readonly RolPermissionData _rolPermissionData;
        private readonly ILogger _logger;

        public RolPermissionBusiness(RolPermissionData rolPermissionData, ILogger logger)
        {
            _rolPermissionData = rolPermissionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos de roles como DTOs
        public async Task<IEnumerable<RolPermissionDTO>> GetAllRolPermisionsAsync()
        {
            try
            {
                var rolPermissions = await _rolPermissionData.GetAllAsync();
                return rolPermissions.Select(rolPermission => new RolPermissionDTO
                {
                    Id = rolPermission.Id,
                    RolId = rolPermission.RolId,
                    PermissionId = rolPermission.PermissionId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos de roles", ex);
            }
        }

        // Método para obtener un permiso de rol por ID como DTO
        public async Task<RolPermissionDTO> GetRolPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso de rol con ID inválido: {RolPermisionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso de rol debe ser mayor que cero");
            }

            try
            {
                var rolPermission = await _rolPermissionData.GetByIdAsync(id);
                if (rolPermission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso de rol con ID: {RolPermisionId}", id);
                    throw new EntityNotFoundException("RolPermision", id);
                }

                return new RolPermissionDTO
                {
                    Id = rolPermission.Id,
                    RolId = rolPermission.RolId,
                    PermissionId = rolPermission.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID: {RolPermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso de rol con ID {id}", ex);
            }
        }

        // Método para crear un permiso de rol desde un DTO
        public async Task<RolPermissionDTO> CreateRolPermisionAsync(RolPermissionDTO rolPermissionDTO)
        {
            try
            {
                ValidateRolPermision(rolPermissionDTO);

                var rolPermision = new RolPermission
                {
                    RolId = rolPermissionDTO.RolId,
                    PermissionId = rolPermissionDTO.PermissionId
                };

                var createdRolPermision = await _rolPermissionData.CreateAsync(rolPermision);

                return new RolPermissionDTO
                {
                    Id = createdRolPermision.Id,
                    RolId = createdRolPermision.RolId,
                    PermissionId = createdRolPermision.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso de rol: {RolId}-{PermisionId}", rolPermissionDTO?.RolId, rolPermissionDTO?.PermissionId);
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso de rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermision(RolPermissionDTO rolPermissionDTO)
        {
            if (rolPermissionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso de rol no puede ser nulo");
            }

            if (rolPermissionDTO.RolId <= 0 || rolPermissionDTO.PermissionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso de rol con valores inválidos");
                throw new Utilities.Exceptions.ValidationException("RolId/PermisionId", "Los IDs del rol y permiso deben ser mayores que cero");
            }
        }
    }
}
