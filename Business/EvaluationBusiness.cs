using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;


namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las evaluaciones en el sistema.
    /// </summary>
    public class EvaluationBusiness
    {
        private readonly EvaluationData _evaluationData;
        private readonly ILogger _logger;

        public EvaluationBusiness(EvaluationData evaluationData, ILogger logger)
        {
            _evaluationData = evaluationData;
            _logger = logger;
        }

        // Método para obtener todas las evaluaciones como DTOs
        public async Task<IEnumerable<EvaluationDTO>> GetAllEvaluationsAsync()
        {
            try
            {
                var evaluations = await _evaluationData.GetAllAsync();
                return MapToDTOList(evaluations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las evaluaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de evaluaciones", ex);
            }
        }

        // Método para obtener una evaluación por ID como DTO
        public async Task<EvaluationDTO> GetEvaluationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una evaluación con ID inválido: {EvaluationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la evaluación debe ser mayor que cero");
            }

            try
            {
                var evaluation = await _evaluationData.GetByIdAsync(id);
                if (evaluation == null)
                {
                    _logger.LogInformation("No se encontró ninguna evaluación con ID: {EvaluationId}", id);
                    throw new EntityNotFoundException("Evaluation", id);
                }

                return MapToDTO(evaluation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la evaluación con ID: {EvaluationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la evaluación con ID {id}", ex);
            }
        }

        // Método para crear una evaluación desde un DTO
        public async Task<EvaluationDTO> CreateEvaluationAsync(EvaluationDTO evaluationDto)
        {
            try
            {
                ValidateEvaluation(evaluationDto);

               var evaluation = MapToEntity(evaluationDto);

                var createdEvaluation = await _evaluationData.CreateAsync(evaluation);

               return MapToDTO(createdEvaluation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva evaluación: {EvaluationName}", evaluationDto?.TypeEvaluation ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluation(EvaluationDTO evaluationDto)
        {
            if (evaluationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(evaluationDto.TypeEvaluation))
            {
                _logger.LogWarning("Se intentó crear/actualizar una evaluación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la evaluación es obligatorio");
            }
        }

        //Metoddo para mapear un Evaluation a EvaluationDTO
        private EvaluationDTO MapToDTO(Evaluation evaluation)
        {
            return new EvaluationDTO
            {
                Id = evaluation.Id,
                TypeEvaluation = evaluation.TypeEvaluation,
                Comments = evaluation.Comments,
                DataTime = evaluation.DataTime,
                UserId1 = evaluation.UserId1,
                ExperienceId1 = evaluation.ExperienceId1
            };
        }

        //Metodo para mapear de EvaluationDTO a Evaluation
        private Evaluation MapToEntity(EvaluationDTO evaluationDto)
        {
            return new Evaluation
            {
                Id = evaluationDto.Id,
                TypeEvaluation = evaluationDto.TypeEvaluation,
                Comments = evaluationDto.Comments,
                DataTime = evaluationDto.DataTime,
                UserId1 = evaluationDto.UserId1,
                ExperienceId1 = evaluationDto.ExperienceId1
            };
        }

        //Metodo para mapear una lista de Evaluation a una lista de EvaluationDTO
        private IEnumerable<EvaluationDTO> MapToDTOList(IEnumerable<Evaluation> evaluations)
        {
            var evaluationsDto = new List<EvaluationDTO>();
            foreach (var evaluation in evaluations)
            {
                evaluationsDto.Add(MapToDTO(evaluation));
            }
            return evaluationsDto;
        }
    }
}
