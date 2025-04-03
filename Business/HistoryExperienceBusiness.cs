using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con el historial de experiencias en el sistema.
    /// </summary>
    public class HistoryExperienceBusiness
    {
        private readonly HistoryExperienceData _historyExperienceData;
        private readonly ILogger _logger;

        public HistoryExperienceBusiness(HistoryExperienceData historyExperienceData, ILogger logger)
        {
            _historyExperienceData = historyExperienceData;
            _logger = logger;
        }

        // Método para obtener todo el historial de experiencias como DTOs
        public async Task<IEnumerable<HistoryExperienceDTO>> GetAllHistoryExperiencesAsync()
        {
            try
            {
                var histories = await _historyExperienceData.GetAllAsync();
                return histories.Select(history => new HistoryExperienceDTO
                {
                    Id = history.Id,
                    ExperienceId = history.ExperienceId,
                    UserId = history.UserId,
                    Action = history.Action,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar el historial de experiencias", ex);
            }
        }

        // Método para obtener un historial de experiencia por ID como DTO
        public async Task<HistoryExperienceDTO> GetHistoryExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un historial de experiencia con ID inválido: {HistoryId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del historial de experiencia debe ser mayor que cero");
            }

            try
            {
                var history = await _historyExperienceData.GetByIdAsync(id);
                if (history == null)
                {
                    _logger.LogInformation("No se encontró ningún historial de experiencia con ID: {HistoryId}", id);
                    throw new EntityNotFoundException("HistoryExperience", id);
                }

                return new HistoryExperienceDTO
                {
                    Id = history.Id,
                    ExperienceId = history.ExperienceId,
                    UserId = history.UserId,
                    Action = history.Action,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencia con ID: {HistoryId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de experiencia con ID {id}", ex);
            }
        }

        // Método para registrar una nueva entrada en el historial de experiencias
        public async Task<HistoryExperienceDTO> CreateHistoryExperienceAsync(HistoryExperienceDTO historyDTO)
        {
            try
            {
                ValidateHistoryExperience(historyDTO);

                var history = new HistoryExperience
                {
                    ExperienceId = historyDTO.ExperienceId,
                    UserId = historyDTO.UserId,
                    Action = historyDTO.Action,
                };

                var createdHistory = await _historyExperienceData.CreateAsync(history);

                return new HistoryExperienceDTO
                {
                    Id = createdHistory.Id,
                    ExperienceId = createdHistory.ExperienceId,
                    UserId = createdHistory.UserId,
                    Action = createdHistory.Action,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar una nueva entrada en el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al registrar el historial de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateHistoryExperience(HistoryExperienceDTO historyDTO)
        {
            if (historyDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto historial de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(historyDTO.Name))
            {
                _logger.LogWarning("Se intentó registrar un historial de experiencia con ExperienceId inválido");
                throw new Utilities.Exceptions.ValidationException("ExperienceId", "El ExperienceId debe ser mayor que cero");
            }

        }
    }
}
