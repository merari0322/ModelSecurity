using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los criterios en el sistema.
    /// </summary>
    public class CriteriaBusiness
    {
        private readonly CriteriaData _criteriaData;
        private readonly ILogger _logger;

        public CriteriaBusiness(CriteriaData criteriaData, ILogger logger)
        {
            _criteriaData = criteriaData;
            _logger = logger;
        }

        // Método para obtener todos los criterios como DTOs
        public async Task<IEnumerable<CriteriaDTO>> GetAllCriteriaAsync()
        {
            try
            {
                var criteria = await _criteriaData.GetAllAsync();
                return MapToDTOList(criteria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los criterios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de criterios", ex);
            }
        }

        // Método para obtener un criterio por ID como DTO
        public async Task<CriteriaDTO> GetCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un criterio con ID inválido: {CriteriaId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del criterio debe ser mayor que cero");
            }

            try
            {
                var criterion = await _criteriaData.GetByIdAsync(id);
                if (criterion == null)
                {
                    _logger.LogInformation("No se encontró ningún criterio con ID: {CriteriaId}", id);
                    throw new EntityNotFoundException("Criteria", id);
                }

                return MapToDTO(criterion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio con ID: {CriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el criterio con ID {id}", ex);
            }
        }

        // Método para crear un criterio desde un DTO
        public async Task<CriteriaDTO> CreateCriteriaAsync(CriteriaDTO CriteriaDTO)
        {
            try
            {
                ValidateCriteria(CriteriaDTO);

                var criterion = MapToEntity(CriteriaDTO);   

                var createdCriterion = await _criteriaData.CreateAsync(criterion);

                return MapToDTO(createdCriterion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo criterio: {CriteriaName}", CriteriaDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el criterio", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateCriteria(CriteriaDTO CriteriaDTO)
        {
            if (CriteriaDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto criterio no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(CriteriaDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un criterio con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del criterio es obligatorio");
            }
        }

        //Metodo para mapear el Criteria a CriteriaDTO 
        private CriteriaDTO MapToDTO(Criteria criteria)
        {
            return new CriteriaDTO
            {
                Id = criteria.Id,
                Name = criteria.Name,

            };
        }

        // Método para mapear del CriteriaaDTO a Criteria
        private Criteria MapToEntity(CriteriaDTO criteriaDTO)
        {
            return new Criteria
            {
                Id = criteriaDTO.Id,
                Name = criteriaDTO.Name,

            };
        }

        //Metodo para mapear una lista de  Criteria a una lista de CriteriaDTO
        private IEnumerable<CriteriaDTO> MapToDTOList(IEnumerable<Criteria> criterias)
        {
            var criteriaDTOs = new List<CriteriaDTO>();
            foreach (var criteria in criterias)
            {
                criteriaDTOs.Add(MapToDTO(criteria));
            }
            return criteriaDTOs;

        }
    }
}


