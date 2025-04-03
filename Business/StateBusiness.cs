using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los estados en el sistema.
    /// </summary>
    public class StateBusiness
    {
        private readonly StateData _stateData;
        private readonly ILogger _logger;

        public StateBusiness(StateData stateData, ILogger logger)
        {
            _stateData = stateData;
            _logger = logger;
        }

        // Método para obtener todos los estados como DTOs
        public async Task<IEnumerable<StateDTO>> GetAllStatesAsync()
        {
            try
            {
                var states = await _stateData.GetAllAsync();
                return states.Select(state => new StateDTO
                {
                    Id = state.Id,
                    Name = state.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de estados", ex);
            }
        }

        // Método para obtener un estado por ID como DTO
        public async Task<StateDTO> GetStateByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un estado con ID inválido: {StateId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del estado debe ser mayor que cero");
            }

            try
            {
                var state = await _stateData.GetByIdAsync(id);
                if (state == null)
                {
                    _logger.LogInformation("No se encontró ningún estado con ID: {StateId}", id);
                    throw new EntityNotFoundException("State", id);
                }

                return new StateDTO
                {
                    Id = state.Id,
                    Name = state.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estado con ID: {StateId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el estado con ID {id}", ex);
            }
        }

        // Método para crear un estado desde un DTO
        public async Task<StateDTO> CreateStateAsync(StateDTO stateDTO)
        {
            try
            {
                ValidateState(stateDTO);

                var state = new State
                {
                    Name = stateDTO.Name
                };

                var createdState = await _stateData.CreateAsync(state);

                return new StateDTO
                {
                    Id = createdState.Id,
                    Name = createdState.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo estado: {StateName}", stateDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el estado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateState(StateDTO stateDTO)
        {
            if (stateDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto estado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(stateDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un estado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del estado es obligatorio");
            }
        }
    }
}
