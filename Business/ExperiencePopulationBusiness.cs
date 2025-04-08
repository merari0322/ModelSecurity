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

        // Método para obtener todas las poblaciones de experiencia como DTOs
        public async Task<IEnumerable<ExperiencePopulationDTO>> GetAllExperiencePopulationsAsync()
        {
            try
            {
                var experiencePopulations = await _experiencePopulationData.GetAllAsync();
                return MapToDTOList(experiencePopulations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las poblaciones de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de poblaciones de experiencia", ex);
            }
        }

        // Método para obtener una población de experiencia por ID como DTO
        public async Task<ExperiencePopulationDTO> GetExperiencePopulationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una población de experiencia con ID inválido: {ExperiencePopulationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la población de experiencia debe ser mayor que cero");
            }

            try
            {
                var population = await _experiencePopulationData.GetByIdAsync(id);
                if (population == null)
                {
                    _logger.LogInformation("No se encontró ninguna población de experiencia con ID: {ExperiencePopulationId}", id);
                    throw new EntityNotFoundException("ExperiencePopulation", id);
                }
                return MapToDTO(population);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la población de experiencia con ID: {ExperiencePopulationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la población de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una población de experiencia desde un DTO
        public async Task<ExperiencePopulationDTO> CreateExperiencePopulationAsync(ExperiencePopulationDTO experiencePopulationDTO)
        {
            try
            {
                ValidateExperiencePopulation(experiencePopulationDTO);

                var experiencePopulation = MapToEntity(experiencePopulationDTO);
                var createdPopulation = await _experiencePopulationData.CreateAsync(experiencePopulation);

                return MapToDTO(createdPopulation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva población de experiencia: {ExperiencePopulationName}", experiencePopulationDTO?.Name ?? "null");
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

        // Método para mapear de ExperiencePopulation a ExperiencePopulationDTO
        private ExperiencePopulationDTO MapToDTO(ExperiencePopulation population)
        {
            return new ExperiencePopulationDTO
            {
                Id = population.Id,
                Name = population.Name,
                Description = population.Description,
                CreatedAt = population.CreatedAt,
                UpdatedAt = population.UpdatedAt
            };
        }

        // Método para mapear de ExperiencePopulationDTO a ExperiencePopulation
        private ExperiencePopulation MapToEntity(ExperiencePopulationDTO populationDTO)
        {
            return new ExperiencePopulation
            {
                Id = populationDTO.Id,
                Name = populationDTO.Name,
                Description = populationDTO.Description,
                CreatedAt = populationDTO.CreatedAt,
                UpdatedAt = populationDTO.UpdatedAt
            };
        }

        // Método para mapear una lista de ExperiencePopulation a una lista de ExperiencePopulationDTO
        private IEnumerable<ExperiencePopulationDTO> MapToDTOList(IEnumerable<ExperiencePopulation> populations)
        {
            var populationsDTO = new List<ExperiencePopulationDTO>();
            foreach (var population in populations)
            {
                populationsDTO.Add(MapToDTO(population));
            }
            return populationsDTO;
        }
    }
}
