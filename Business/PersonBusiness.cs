using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class PersonBusiness
    {
        private readonly PersonData _personData;
        private readonly ILogger _logger;

        public PersonBusiness(PersonData personData, ILogger logger)
        {
            _personData = personData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync()
        {
            try
            {
                var persons = await _personData.GetAllAsync();
                var personDTO = new List<PersonDTO>();

                foreach (var person in persons)
                {
                    personDTO.Add(new PersonDTO
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Email = person.Email,
                        Phone = person.Phone,
                        Active = person.Active // Si existe en la entidad
                    });
                }

                return personDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PersonDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {PersonId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de person debe ser mayor que cero");
            }

            try
            {
                var person = await _personData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {PersoId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new PersonDTO
                {
                    Id = person.Id,
                    Name = person.Name,
                    Active = person.Active
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {PersonId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el Person con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<PersonDTO> CreatePersonAsync(PersonDTO   PersonDTO)
        {
            try
            {
                ValidatePerson(PersonDTO);

                var person = new Person
                {
                    Name = PersonDTO.Name,
                    Email = PersonDTO.Email,
                    Phone = PersonDTO.Phone,
                    Active = PersonDTO.Active // Si existe en la entidad
                };

                var personCreado = await _personData.CreateAsync(person);

                return new PersonDTO
                {
                    Id = personCreado.Id,
                    Name = personCreado.Name,
                    Active = personCreado.Active // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo person: {personNombre}", PersonDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el person", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePerson(PersonDTO PersonDTO)
        {
            if (PersonDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }
    }
}

