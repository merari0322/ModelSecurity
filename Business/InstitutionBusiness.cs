using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las instituciones en el sistema.
    /// </summary>
    public class InstitutionBusiness
    {
        private readonly InstitutionData _institucionData;
        private readonly ILogger _logger;

        public InstitutionBusiness(InstitutionData institucionData, ILogger logger)
        {
            _institucionData = institucionData;
            _logger = logger;
        }

        // Método para obtener todas las instituciones como DTOs
        public async Task<IEnumerable<InstitutionDTO>> GetAllInstitucionesAsync()
        {
            try
            {
                var instituciones = await _institucionData.GetAllAsync();
                return instituciones.Select(inst => new InstitutionDTO
                {
                    Id = inst.Id,
                    Name = inst.Name,
                    Address = inst.Address,
                    Phone = inst.Phone,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las instituciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de instituciones", ex);
            }
        }

        // Método para obtener una institución por ID como DTO
        public async Task<InstitutionDTO> GetInstitucionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una institución con ID inválido: {InstitucionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la institución debe ser mayor que cero");
            }

            try
            {
                var institucion = await _institucionData.GetByIdAsync(id);
                if (institucion == null)
                {
                    _logger.LogInformation("No se encontró ninguna institución con ID: {InstitucionId}", id);
                    throw new EntityNotFoundException("Institucion", id);
                }

                return new InstitutionDTO
                {
                    Id = institucion.Id,
                    Name = institucion.Name,
                    Address = institucion.Address,
                    Phone = institucion.Phone,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la institución con ID: {InstitucionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la institución con ID {id}", ex);
            }
        }

        // Método para crear una institución desde un DTO
        public async Task<InstitutionDTO> CreateInstitucionAsync(InstitutionDTO institucionDto)
        {
            try
            {
                ValidateInstitucion(institucionDto);

                var institucion = new Institution
                {
                    Name = institucionDto.Name,
                    Address = institucionDto.Address,
                    Phone = institucionDto.Phone,
                };

                var createdInstitucion = await _institucionData.CreateAsync(institucion);

                return new InstitutionDTO
                {
                    Id = createdInstitucion.Id,
                    Name = createdInstitucion.Name,
                    Address = createdInstitucion.Address,
                    Phone = createdInstitucion.Phone,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva institución: {InstitucionName}", institucionDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la institución", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateInstitucion(InstitutionDTO institucionDto)
        {
            if (institucionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto institución no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(institucionDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una institución con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la institución es obligatorio");
            }
        }
    }
}
