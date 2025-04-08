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
        public async Task<IEnumerable<RolPermissionDTO>> GetAllRolPermissionsAsync()
        {
            try
            {
                var rolPermissions = await _rolPermissionData.GetAllAsync();
                return MapToDTOList(rolPermissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos de roles", ex);
            }
        }

        // Método para obtener un permiso de rol por ID como DTO
        public async Task<RolPermissionDTO> GetRolPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso de rol con ID inválido: {RolPermissionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso de rol debe ser mayor que cero");
            }

            try
            {
                var rolPermission = await _rolPermissionData.GetByIdAsync(id);
                if (rolPermission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso de rol con ID: {RolPermissionId}", id);
                    throw new EntityNotFoundException("RolPermission", id);
                }

                return MapToDTO(rolPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID: {RolPermissionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso de rol con ID {id}", ex);
            }
        }

        // Método para crear un permiso de rol desde un DTO
        public async Task<RolPermissionDTO> CreateRolPermissionAsync(RolPermissionDTO rolPermissionDTO)
        {
            try
            {
                ValidateRolPermission(rolPermissionDTO);

                var rolPermission = MapToEntity(rolPermissionDTO);
                var createdRolPermission = await _rolPermissionData.CreateAsync(rolPermission);

                return MapToDTO(createdRolPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso de rol: {RolId}-{PermissionId}", rolPermissionDTO?.RolId, rolPermissionDTO?.PermissionId);
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso de rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermission(RolPermissionDTO rolPermissionDTO)
        {
            if (rolPermissionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso de rol no puede ser nulo");
            }

            if (rolPermissionDTO.RolId <= 0 || rolPermissionDTO.PermissionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso de rol con valores inválidos");
                throw new Utilities.Exceptions.ValidationException("RolId/PermissionId", "Los IDs del rol y permiso deben ser mayores que cero");
            }
        }

        // Método para mapear de RolPermission a RolPermissionDTO
        private RolPermissionDTO MapToDTO(RolPermission rolPermission)
        {
            return new RolPermissionDTO
            {
                Id = rolPermission.Id,
                RolId = rolPermission.RolId,
                PermissionId = rolPermission.PermissionId
            };
        }

        // Método para mapear de RolPermissionDTO a RolPermission
        private RolPermission MapToEntity(RolPermissionDTO rolPermissionDTO)
        {
            return new RolPermission
            {
                Id = rolPermissionDTO.Id,
                RolId = rolPermissionDTO.RolId,
                PermissionId = rolPermissionDTO.PermissionId
            };
        }

        // Método para mapear una lista de RolPermission a una lista de RolPermissionDTO
        private IEnumerable<RolPermissionDTO> MapToDTOList(IEnumerable<RolPermission> rolPermissions)
        {
            var rolPermissionsDTO = new List<RolPermissionDTO>();
            foreach (var rolPermission in rolPermissions)
            {
                rolPermissionsDTO.Add(MapToDTO(rolPermission));
            }
            return rolPermissionsDTO;
        }
    }
}



