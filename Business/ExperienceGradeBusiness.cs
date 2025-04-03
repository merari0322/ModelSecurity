using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de experiencia en el sistema.
    /// </summary>
    public class ExperienceGradeBusiness
    {
        private readonly ExperienceGradeData _experienceGradeData;
        private readonly ILogger _logger;

        public ExperienceGradeBusiness(ExperienceGradeData experienceGradeData, ILogger logger)
        {
            _experienceGradeData = experienceGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de experiencia como DTOs
        public async Task<IEnumerable<ExperienceGradeDTO>> GetAllExperienceGradesAsync()
        {
            try
            {
                var experienceGrades = await _experienceGradeData.GetAllAsync();
                return experienceGrades.Select(experienceGrade => new ExperienceGradeDTO
                {
                    Id = experienceGrade.Id,
                    GradeId = experienceGrade.GradeId,
                    ExperienceId = experienceGrade.ExperienceId,
                   
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de experiencia", ex);
            }
        }

        // Método para obtener un grado de experiencia por ID como DTO
        public async Task<ExperienceGradeDTO> GetExperienceGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de experiencia con ID inválido: {ExperienceGradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado de experiencia debe ser mayor que cero");
            }

            try
            {
                var experienceGrade = await _experienceGradeData.GetByIdAsync(id);
                if (experienceGrade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de experiencia con ID: {ExperienceGradeId}", id);
                    throw new EntityNotFoundException("ExperienceGrade", id);
                }

                return new ExperienceGradeDTO
                {
                    Id = experienceGrade.Id,
                    GradeId = experienceGrade.GradeId,
                    ExperienceId = experienceGrade.ExperienceId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de experiencia con ID: {ExperienceGradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de experiencia con ID {id}", ex);
            }
        }

        // Método para crear un grado de experiencia desde un DTO
        public async Task<ExperienceGradeDTO> CreateExperienceGradeAsync(ExperienceGradeDTO experienceGradeDTO)
        {
            try
            {
                ValidateExperienceGrade(experienceGradeDTO);

                var experienceGrade = new ExperienceGrade
                {
                 
                    GradeId = experienceGradeDTO.GradeId,
                    ExperienceId = experienceGradeDTO.ExperienceId,
                };

                var createdExperienceGrade = await _experienceGradeData.CreateAsync(experienceGrade);

                return new ExperienceGradeDTO
                {
                Id = createdExperienceGrade.Id,
                    GradeId = createdExperienceGrade.GradeId,
                    ExperienceId = createdExperienceGrade.ExperienceId,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de experiencia: {ExperienceGradeName}", experienceGradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceGrade(ExperienceGradeDTO experienceGradeDto)
        {
            if (experienceGradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceGradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado de experiencia es obligatorio");
            }
        }
    }
}
