using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las líneas temáticas en el sistema.
    /// </summary>
    public class LineThematicBusiness
    {
        private readonly LineThematicData _lineThematicData;
        private readonly ILogger _logger;

        public LineThematicBusiness(LineThematicData lineThematicData, ILogger logger)
        {
            _lineThematicData = lineThematicData;
            _logger = logger;
        }

        // Método para obtener todas las líneas temáticas como DTOs
        public async Task<IEnumerable<LineThematicDTO>> GetAllLineThematicsAsync()
        {
            try
            {
                var lineThematics = await _lineThematicData.GetAllAsync();
                return lineThematics.Select(lineThematic => new LineThematicDTO
                {
                    Id = lineThematic.Id,
                    Name = lineThematic.Name,
                    Description = lineThematic.Description,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las líneas temáticas");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de líneas temáticas", ex);
            }
        }

        // Método para obtener una línea temática por ID como DTO
        public async Task<LineThematicDTO> GetLineThematicByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una línea temática con ID inválido: {LineThematicId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la línea temática debe ser mayor que cero");
            }

            try
            {
                var lineThematic = await _lineThematicData.GetByIdAsync(id);
                if (lineThematic == null)
                {
                    _logger.LogInformation("No se encontró ninguna línea temática con ID: {LineThematicId}", id);
                    throw new EntityNotFoundException("LineThematic", id);
                }

                return new LineThematicDTO
                {
                    Id = lineThematic.Id,
                    Name = lineThematic.Name,
                    Description = lineThematic.Description,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la línea temática con ID: {LineThematicId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la línea temática con ID {id}", ex);
            }
        }

        // Método para crear una línea temática desde un DTO
        public async Task<LineThematicDTO> CreateLineThematicAsync(LineThematicDTO lineThematicDTO)
        {
            try
            {
                ValidateLineThematic(lineThematicDTO);

                var lineThematic = new LineThematic
                {
                    Name = lineThematicDTO.Name,
                    Description = lineThematicDTO.Description,
                };

                var createdLineThematic = await _lineThematicData.CreateAsync(lineThematic);

                return new LineThematicDTO
                {
                    Id = createdLineThematic.Id,
                    Name = createdLineThematic.Name,
                    Description = createdLineThematic.Description,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva línea temática: {LineThematicName}", lineThematicDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la línea temática", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateLineThematic(LineThematicDTO lineThematicDTO)
        {
            if (lineThematicDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto línea temática no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(lineThematicDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una línea temática con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la línea temática es obligatorio");
            }
        }
    }
}
