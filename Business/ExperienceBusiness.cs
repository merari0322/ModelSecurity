using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las experiencias en el sistema.
    /// </summary>
    public class ExperienceBusiness
    {
        private readonly ExperienceData _experienceData;
        private readonly ILogger _logger;

        public ExperienceBusiness(ExperienceData experienceData, ILogger logger)
        {
            _experienceData = experienceData;
            _logger = logger;
        }

        // Método para obtener todas las experiencias como DTOs
        public async Task<IEnumerable<ExperienceDTO>> GetAllExperiencesAsync()
        {
            try
            {
                var experiences = await _experienceData.GetAllAsync();
                var experienceDTOs = new List<ExperienceDTO>();

                foreach (var experience in experiences)
                {
                    experienceDTOs.Add(new ExperienceDTO
                    {
                        Id = experience.Id,
                        Summary = experience.Summary,
                        Methodologies = experience.Methodologies,
                        Transfe = experience.Transfe,
                        DataRegistration = experience.DataRegistration,
                        UserId1 = experience.UserId1,
                        InstitutionId1 = experience.InstitutionId1
                    });
                }

                return experienceDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de experiencias", ex);
            }
        }

        // Método para obtener una experiencia por ID como DTO
        public async Task<ExperienceDTO> GetExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una experiencia con ID inválido: {ExperienceId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la experiencia debe ser mayor que cero");
            }

            try
            {
                var experience = await _experienceData.GetByIdAsync(id);
                if (experience == null)
                {
                    _logger.LogInformation("No se encontró ninguna experiencia con ID: {ExperienceId}", id);
                    throw new EntityNotFoundException("Experience", id);
                }

                return new ExperienceDTO
                {
                        Id = experience.Id,
                        NameExperience = experience.NameExperience,
                        Summary = experience.Summary,
                        Methodologies = experience.Methodologies,
                        Transfe = experience.Transfe,
                        DataRegistration = experience.DataRegistration,
                        UserId1 = experience.UserId1,
                        InstitutionId1 = experience.InstitutionId1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la experiencia con ID: {ExperienceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la experiencia con ID {id}", ex);
            }
        }

        // Método para crear una experiencia desde un DTO
        public async Task<ExperienceDTO> CreateExperienceAsync(ExperienceDTO experienceDto)
        {
            try
            {
                ValidateExperience(experienceDto);

                var experience = new Experience
                {
                        NameExperience = experienceDto.NameExperience,
                        Summary = experienceDto.Summary,
                        Methodologies = experienceDto.Methodologies,
                        Transfe = experienceDto.Transfe,
                        DataRegistration = experienceDto.DataRegistration,
                        UserId1 = experienceDto.UserId1,
                        InstitutionId1 = experienceDto.InstitutionId1
                };

                var experienceCreated = await _experienceData.CreateAsync(experience);

                return new ExperienceDTO
                {
                      Id = experienceCreated.Id,
                        Summary = experienceCreated.Summary,
                        Methodologies = experienceCreated.Methodologies,
                        Transfe = experienceCreated.Transfe,
                        DataRegistration = experienceCreated.DataRegistration,
                        UserId1 = experienceCreated.UserId1,
                        InstitutionId1 = experienceCreated.InstitutionId1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva experiencia: {ExperienceTitle}", experienceDto?.NameExperience ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperience(ExperienceDTO experienceDto)
        {
            if (experienceDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceDto.NameExperience))
            {
                _logger.LogWarning("Se intentó crear/actualizar una experiencia con Title vacío");
                throw new Utilities.Exceptions.ValidationException("Title", "El Title de la experiencia es obligatorio");
            }
        }
    }
}
