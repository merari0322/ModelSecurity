using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los objetivos en el sistema.
    /// </summary>
    public class ObjectiveBusiness
    {
        private readonly ObjectiveData _objectiveData;
        private readonly ILogger _logger;

        public ObjectiveBusiness(ObjectiveData objectiveData, ILogger logger)
        {
            _objectiveData = objectiveData;
            _logger = logger;
        }

        // Método para obtener todos los objetivos como DTOs
        public async Task<IEnumerable<ObjectiveDTO>> GetAllObjectivesAsync()
        {
            try
            {
                var objectives = await _objectiveData.GetAllAsync();
                var objectiveDTOs = objectives.Select(obj => new ObjectiveDTO
                {
                    Id = obj.Id,
                    ObjectiveDescription = obj.ObjectiveDescription,
                    Innovation = obj.Innovation,
                    Results = obj.Results,
                    Sustainability = obj.Sustainability

                }).ToList();

                return objectiveDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los objetivos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de objetivos", ex);
            }
        }

        // Método para obtener un objetivo por ID como DTO
        public async Task<ObjectiveDTO> GetObjectiveByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un objetivo con ID inválido: {ObjectiveId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del objetivo debe ser mayor que cero");
            }

            try
            {
                var objective = await _objectiveData.GetByIdAsync(id);
                if (objective == null)
                {
                    _logger.LogInformation("No se encontró ningún objetivo con ID: {ObjectiveId}", id);
                    throw new EntityNotFoundException("Objective", id);
                }

                return new ObjectiveDTO
                {
                    Id = objective.Id,
                    ObjectiveDescription = objective.ObjectiveDescription,
                    Innovation = objective.Innovation,
                    Results = objective.Results,
                    Sustainability = objective.Sustainability
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el objetivo con ID: {ObjectiveId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el objetivo con ID {id}", ex);
            }
        }

        // Método para crear un objetivo desde un DTO
        public async Task<ObjectiveDTO> CreateObjectiveAsync(ObjectiveDTO objectiveDTO)
        {
            try
            {
                ValidateObjective(objectiveDTO);

                var objective = new Objective
                {
                    Id = objectiveDTO.Id,
                    ObjectiveDescription = objectiveDTO.ObjectiveDescription,
                    Innovation = objectiveDTO.Innovation,
                    Results = objectiveDTO.Results,
                    Sustainability = objectiveDTO.Sustainability,
                };

                var createdObjective = await _objectiveData.CreateAsync(objective);

                return new ObjectiveDTO
                {
                    Id = createdObjective.Id,
                    ObjectiveDescription = createdObjective.ObjectiveDescription,
                    Innovation = createdObjective.Innovation,
                    Results = createdObjective.Results,
                    Sustainability = createdObjective.Sustainability
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo objetivo: {ObjectiveName}", objectiveDTO?.ObjectiveDescription ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el objetivo", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateObjective(ObjectiveDTO objectiveDTO)
        {
            if (objectiveDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto objetivo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(objectiveDTO.ObjectiveDescription))
            {
                _logger.LogWarning("Se intentó crear/actualizar un objetivo con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del objetivo es obligatorio");
            }
        }
    }
}
