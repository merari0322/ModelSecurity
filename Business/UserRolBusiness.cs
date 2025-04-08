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
    /// Clase de negocio encargada de la lógica relacionada con los roles de usuario del sistema.
    /// </summary>
    public class UserRolBusiness
    {
        private readonly UserRolData _userRolData;
        private readonly ILogger _logger;

        public UserRolBusiness(UserRolData userRolData, ILogger logger)
        {
            _userRolData = userRolData;
            _logger = logger;
        }

        // Método para obtener todos los roles de usuario como DTOs
        public async Task<IEnumerable<UserRolDTO>> GetAllUserRolesAsync()
        {
            try
            {
                var userRoles = await _userRolData.GetAllAsync();
                return MapToDTOList(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles de usuario", ex);
            }
        }

        // Método para obtener un rol de usuario por ID como DTO
        public async Task<UserRolDTO> GetUserRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol de usuario con ID inválido: {UserRolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol de usuario debe ser mayor que cero");
            }

            try
            {
                var userRol = await _userRolData.GetByIdAsync(id);
                if (userRol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol de usuario con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                return MapToDTO(userRol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol de usuario con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol de usuario con ID {id}", ex);
            }
        }

        // Método para crear un rol de usuario desde un DTO
        public async Task<UserRolDTO> CreateUserRolAsync(UserRolDTO userRolDTO)
        {
            try
            {
                ValidateUserRol(userRolDTO);

                var userRol = MapToEntity(userRolDTO);
                var createdUserRol = await _userRolData.CreateAsync(userRol);

                return MapToDTO(createdUserRol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo rol de usuario: {UserRolName}", userRolDTO?.RolId.ToString() ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol de usuario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUserRol(UserRolDTO userRolDTO)
        {
            if (userRolDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol de usuario no puede ser nulo");
            }

            if (userRolDTO.UserId <= 0 || userRolDTO.RolId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol de usuario con valores inválidos");
                throw new Utilities.Exceptions.ValidationException("UserId/RolId", "Los IDs del usuario y rol deben ser mayores que cero");
            }
        }

        // Método para mapear de UserRol a UserRolDTO
        private UserRolDTO MapToDTO(UserRol userRol)
        {
            return new UserRolDTO
            {
                Id = userRol.Id,
                UserId = userRol.UserId,
                RolId = userRol.RolId
            };
        }

        // Método para mapear de UserRolDTO a UserRol
        private UserRol MapToEntity(UserRolDTO userRolDTO)
        {
            return new UserRol
            {
                Id = userRolDTO.Id,
                UserId = userRolDTO.UserId,
                RolId = userRolDTO.RolId
            };
        }

        // Método para mapear una lista de UserRol a una lista de UserRolDTO
        private IEnumerable<UserRolDTO> MapToDTOList(IEnumerable<UserRol> userRoles)
        {
            var userRolesDTO = new List<UserRolDTO>();
            foreach (var userRol in userRoles)
            {
                userRolesDTO.Add(MapToDTO(userRol));
            }
            return userRolesDTO;
        }
    }
}




