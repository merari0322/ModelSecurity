using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las instituciones en el sistema.
    /// </summary>
    public class InstitutionBusiness
    {
        private readonly InstitutionData _institutionData;
        private readonly ILogger _logger;

        public InstitutionBusiness(InstitutionData institutionData, ILogger logger)
        {
            _institutionData = institutionData;
            _logger = logger;
        }

        // Método para obtener todas las instituciones como DTOs
        public async Task<IEnumerable<InstitutionDTO>> GetAllInstitutionsAsync()
        {
            try
            {
                var institutions = await _institutionData.GetAllAsync();
                return MapToDTOList(institutions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las instituciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de instituciones", ex);
            }
        }

        // Método para obtener una institución por ID como DTO
        public async Task<InstitutionDTO> GetInstitutionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una institución con ID inválido: {InstitutionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la institución debe ser mayor que cero");
            }

            try
            {
                var institution = await _institutionData.GetByIdAsync(id);
                if (institution == null)
                {
                    _logger.LogInformation("No se encontró ninguna institución con ID: {InstitutionId}", id);
                    throw new EntityNotFoundException("Institution", id);
                }

                return MapToDTO(institution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la institución con ID: {InstitutionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la institución con ID {id}", ex);
            }
        }

        // Método para crear una institución desde un DTO
        public async Task<InstitutionDTO> CreateInstitutionAsync(InstitutionDTO institutionDto)
        {
            try
            {
                ValidateInstitution(institutionDto);

                var institution = MapToEntity(institutionDto);
                var createdInstitution = await _institutionData.CreateAsync(institution);

                return MapToDTO(createdInstitution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva institución: {InstitutionName}", institutionDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la institución", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateInstitution(InstitutionDTO institutionDto)
        {
            if (institutionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto institución no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(institutionDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una institución con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la institución es obligatorio");
            }
        }

        // Método para mapear de Institution a InstitutionDTO
        private InstitutionDTO MapToDTO(Institution institution)
        {
            return new InstitutionDTO
            {
                Id = institution.Id,
                Name = institution.Name,
                Address = institution.Address,
                Phone = institution.Phone,
                EmailInstitution = institution.EmailInstitution,
                Department = institution.Department,
                Commune = institution.Commune
            };
        }

        // Método para mapear de InstitutionDTO a Institution
        private Institution MapToEntity(InstitutionDTO institutionDto)
        {
            return new Institution
            {
                Id = institutionDto.Id,
                Name = institutionDto.Name,
                Address = institutionDto.Address,
                Phone = institutionDto.Phone,
                EmailInstitution = institutionDto.EmailInstitution,
                Department = institutionDto.Department,
                Commune = institutionDto.Commune
            };
        }

        // Método para mapear una lista de Institution a una lista de InstitutionDTO
        private IEnumerable<InstitutionDTO> MapToDTOList(IEnumerable<Institution> institutions)
        {
            var institutionsDTO = new List<InstitutionDTO>();
            foreach (var institution in institutions)
            {
                institutionsDTO.Add(MapToDTO(institution));
            }
            return institutionsDTO;
        }
    }
}

