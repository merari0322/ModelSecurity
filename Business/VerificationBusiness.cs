using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las verificaciones en el sistema.
    /// </summary>
    public class VerificationBusiness
    {
        private readonly VerificationData _verificationData;
        private readonly ILogger _logger;

        public VerificationBusiness(VerificationData verificationData, ILogger logger)
        {
            _verificationData = verificationData;
            _logger = logger;
        }

        // Método para obtener todas las verificaciones como DTOs
        public async Task<IEnumerable<VerificationDTO>> GetAllVerificationsAsync()
        {
            try
            {
                var verifications = await _verificationData.GetAllAsync();
                return MapToDTOList(verifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las verificaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de verificaciones", ex);
            }
        }

        // Método para obtener una verificación por ID como DTO
        public async Task<VerificationDTO> GetVerificationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una verificación con ID inválido: {VerificationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la verificación debe ser mayor que cero");
            }

            try
            {
                var verification = await _verificationData.GetByIdAsync(id);
                if (verification == null)
                {
                    _logger.LogInformation("No se encontró ninguna verificación con ID: {VerificationId}", id);
                    throw new EntityNotFoundException("Verification", id);
                }

                return MapToDTO(verification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la verificación con ID: {VerificationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la verificación con ID {id}", ex);
            }
        }

        // Método para crear una verificación desde un DTO
        public async Task<VerificationDTO> CreateVerificationAsync(VerificationDTO verificationDTO)
        {
            try
            {
                ValidateVerification(verificationDTO);

                var verification = MapToEntity(verificationDTO);
                var createdVerification = await _verificationData.CreateAsync(verification);

                return MapToDTO(createdVerification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva verificación: {VerificationName}", verificationDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la verificación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateVerification(VerificationDTO verificationDTO)
        {
            if (verificationDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto verificación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(verificationDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una verificación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la verificación es obligatorio");
            }
        }

        // Método para mapear de Verification a VerificationDTO
        private VerificationDTO MapToDTO(Verification verification)
        {
            return new VerificationDTO
            {
                Id = verification.Id,
                Name = verification.Name,
                Description = verification.Description
            };
        }

        // Método para mapear de VerificationDTO a Verification
        private Verification MapToEntity(VerificationDTO verificationDTO)
        {
            return new Verification
            {
                Id = verificationDTO.Id,
                Name = verificationDTO.Name,
                Description = verificationDTO.Description
            };
        }

        // Método para mapear una lista de Verification a una lista de VerificationDTO
        private IEnumerable<VerificationDTO> MapToDTOList(IEnumerable<Verification> verifications)
        {
            var verificationsDTO = new List<VerificationDTO>();
            foreach (var verification in verifications)
            {
                verificationsDTO.Add(MapToDTO(verification));
            }
            return verificationsDTO;
        }
    }
}




