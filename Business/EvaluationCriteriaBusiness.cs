using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los criterios de evaluación en el sistema.
    /// </summary>
    public class EvaluationCriteriaBusiness
    {
        private readonly EvaluationCriteriaData _evaluationCriteriaData;
        private readonly ILogger _logger;

        public EvaluationCriteriaBusiness(EvaluationCriteriaData evaluationCriteriaData, ILogger logger)
        {
            _evaluationCriteriaData = evaluationCriteriaData;
            _logger = logger;
        }

        // Método para obtener todos los criterios de evaluación como DTOs
        public async Task<IEnumerable<EvaluationCriteriaDTO>> GetAllEvaluationCriteriaAsync()
        {
            try
            {
                var criteria = await _evaluationCriteriaData.GetAllAsync();
                return criteria.Select(criterion => new EvaluationCriteriaDTO
                {
                    Id = criterion.Id,
                    Score = criterion.Score,
                    EvaluationId = criterion.EvaluationId
                   
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los criterios de evaluación");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de criterios de evaluación", ex);
            }
        }

        // Método para obtener un criterio de evaluación por ID como DTO
        public async Task<EvaluationCriteriaDTO> GetEvaluationCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un criterio de evaluación con ID inválido: {CriteriaId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del criterio de evaluación debe ser mayor que cero");
            }

            try
            {
                var criterion = await _evaluationCriteriaData.GetByIdAsync(id);
                if (criterion == null)
                {
                    _logger.LogInformation("No se encontró ningún criterio de evaluación con ID: {CriteriaId}", id);
                    throw new EntityNotFoundException("EvaluationCriteria", id);
                }

                return new EvaluationCriteriaDTO
                {
                   Id = criterion.Id,
                    Score = criterion.Score,
                    EvaluationId = criterion.EvaluationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio de evaluación con ID: {CriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el criterio de evaluación con ID {id}", ex);
            }
        }

        // Método para crear un criterio de evaluación desde un DTO
        public async Task<EvaluationCriteriaDTO> CreateEvaluationCriteriaAsync(EvaluationCriteriaDTO criteriaDTO)
        {
            try
            {
                ValidateEvaluationCriteria(criteriaDTO);

                var Criteria = new EvaluationCriteria
                {
                    Score = criteriaDTO.Score,
                    EvaluationId = criteriaDTO.EvaluationId,
                   
                };

                var createdCriterion = await _evaluationCriteriaData.CreateAsync(Criteria);

                return new EvaluationCriteriaDTO
                {
                    Id = createdCriterion.Id,
                    Score = createdCriterion.Score,
                    EvaluationId = createdCriterion.EvaluationId,
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo criterio de evaluación: {CriteriaName}", criteriaDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el criterio de evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluationCriteria(EvaluationCriteriaDTO CriteriaDto)
        {
            if (CriteriaDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto criterio de evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(CriteriaDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un criterio de evaluación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del criterio de evaluación es obligatorio");
            }

        }
    }
}
