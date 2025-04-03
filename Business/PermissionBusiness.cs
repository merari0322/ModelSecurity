using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;


namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos en el sistema.
    /// </summary>
    public class PermissionBusiness
    {
        private readonly PermissionData _permisionData;
        private readonly ILogger _logger;

        public PermissionBusiness(PermissionData permisionData, ILogger logger)
        {
            _permisionData = permisionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos como DTOs
        public async Task<IEnumerable<PermissionDTO>> GetAllPermisionsAsync()
        {
            try
            {
                var permisions = await _permisionData.GetAllAsync();
                return permisions.Select(permision => new PermissionDTO
                {
                    Id = permision.Id,
                    Permissiontype = permision.Permissiontype,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        // Método para obtener un permiso por ID como DTO
        public async Task<PermissionDTO> GetPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermisionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permission = await _permisionData.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermisionId}", id);
                    throw new EntityNotFoundException("Permision", id);
                }

                return new PermissionDTO
                {
                    Id = permission.Id,
                    Permissiontype = permission.Permissiontype
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        // Método para crear un permiso desde un DTO
        public async Task<PermissionDTO> CreatePermisionAsync(PermissionDTO permissionDTO)
        {
            try
            {
                ValidatePermision(permissionDTO);

                var permision = new Permission
                {
                    Permissiontype = permissionDTO.Permissiontype,
                };

                var createdPermision = await _permisionData.CreateAsync(permision);

                return new PermissionDTO
                {
                    Id = createdPermision.Id,
                    Permissiontype = createdPermision.Permissiontype
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso: {PermisionName}", permissionDTO?.Permissiontype ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePermision(PermissionDTO permissionDTO)
        {
            if (permissionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(permissionDTO.Permissiontype))
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del permiso es obligatorio");
            }
        }
    }
}
