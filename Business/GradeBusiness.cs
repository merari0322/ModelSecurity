using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados en el sistema.
    /// </summary>
    public class GradeBusiness
    {
        private readonly GradeData _gradeData;
        private readonly ILogger _logger;

        public GradeBusiness(GradeData gradeData, ILogger logger)
        {
            _gradeData = gradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados como DTOs
        public async Task<IEnumerable<GradeDTO>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _gradeData.GetAllAsync();
                return MapToDTOList(grades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados", ex);
            }
        }

        // Método para obtener un grado por ID como DTO
        public async Task<GradeDTO> GetGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado con ID inválido: {GradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado debe ser mayor que cero");
            }

            try
            {
                var grade = await _gradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado con ID: {GradeId}", id);
                    throw new EntityNotFoundException("Grade", id);
                }

                return MapToDTO(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado con ID {id}", ex);
            }
        }

        // Método para crear un grado desde un DTO
        public async Task<GradeDTO> CreateGradeAsync(GradeDTO gradeDTO)
        {
            try
            {
                ValidateGrade(gradeDTO);

                var grade = MapToEntity(gradeDTO);

                var createdGrade = await _gradeData.CreateAsync(grade);

                return MapToDTO(createdGrade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado: {GradeName}", gradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateGrade(GradeDTO gradeDTO)
        {
            if (gradeDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private GradeDTO MapToDTO(Grade grade)
        {
            return new GradeDTO
            {
                Id = grade.Id,
                Name = grade.Name,
                Level = grade.Level
            };
        }

        // Método para mapear de RolDTO a Rol
        private Grade MapToEntity(GradeDTO gradeDTO)
        {
            return new Grade
            {
                Id = gradeDTO.Id,
                Name = gradeDTO.Name,
                Level = gradeDTO.Level,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<GradeDTO> MapToDTOList(IEnumerable<Grade> grades)
        {
            var gradesDTO = new List<GradeDTO>();
            foreach (var grade in grades)
            {
                gradesDTO.Add(MapToDTO(grade));
            }
            return gradesDTO;
        }
    }
}