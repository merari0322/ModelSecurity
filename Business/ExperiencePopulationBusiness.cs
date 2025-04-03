using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con la población de experiencias en el sistema.
    /// </summary>
    public class ExperiencePopulationBusiness 
    {
        private readonly ExperiencePopulationData _experiencePopulationData;
        private readonly ILogger _logger;

        public ExperiencePopulationBusiness(ExperiencePopulationData experiencePopulationData, ILogger logger)
        {
            _experiencePopulationData = experiencePopulationData;
            _logger = logger;
        }

        // Método para obtener una población de experiencia por ID como DTO
        public async Task<ExperiencePopulationDTO> GetExperiencePopulationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una población de experiencia con ID inválido: {PopulationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la población de experiencia debe ser mayor que cero");
            }

            try
            {
                var population = await _experiencePopulationData.GetByIdAsync(id);
                if (population == null)
                {
                    _logger.LogInformation("No se encontró ninguna población de experiencia con ID: {PopulationId}", id);
                    throw new EntityNotFoundException("ExperiencePopulation", id);
                }

                return new ExperiencePopulationDTO
                {
                    Id = population.Id,
                    Name = population.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la población de experiencia con ID: {PopulationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la población de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una población de experiencia desde un DTO
        public async Task<ExperiencePopulationDTO> CreateExperiencePopulationAsync(ExperiencePopulationDTO populationDTO)
        {
            try
            {
                ValidateExperiencePopulation(populationDTO);

                var population = new ExperiencePopulation
                {
                    Name = populationDTO.Name
                };

                var createdPopulation = await _experiencePopulationData.CreateAsync(population);

                return new ExperiencePopulationDTO
                {
                    Id = createdPopulation.Id,
                    Name = createdPopulation.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva población de experiencia: {PopulationName}", populationDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la población de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperiencePopulation(ExperiencePopulationDTO populationDTO)
        {
            if (populationDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto población de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(populationDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una población de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la población de experiencia es obligatorio");
            }
        }
    }
}
