
using Entity.Contexs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
   public class CriteriaData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CriteriaData> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public CriteriaData(ApplicationDbContext context, ILogger<CriteriaData> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los criterios almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<Criteria>> GetAllAsync()
        {
            return await _context.Set<Criteria>().ToListAsync();
        }

        ///<summary> Obtiene un Criterio específico por su identificador.

        public async Task<Criteria?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Criteria>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ron con ID {CriteriaId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo criterio en la base de datos.
        ///</summary>
        ///<param name="criteria">Instancia del criterio a crear</param>
        ///<returns>El criterio creado</returns>

        public async Task<Criteria> CreateAsync(Criteria criteria)
        {
            try
            {
                await _context.Set<Criteria>().AddAsync(criteria);
                await _context.SaveChangesAsync();
                return criteria;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la criteria:{ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Actualiza un criterio existente en la base de datos.
        ///</summary>
        ///<param name="criteria">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Criteria criteria)
        {
            try
            {
                _context.Set<Criteria>().Update(criteria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }

        ///<summary>
        ///Elimina un criterio de la base de datos.
        ///</summary>
        ///<param name="id">Identificador único del criterio a eliminar</param>
        ///<returns>True si la eliminación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var criteria = await _context.Set<Criteria>().FindAsync(id);
                if (criteria == null)
                    return false;

                _context.Set<Criteria>().Remove(criteria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Error al eliminar la criteria: {ex.Message}");
                    return false;
                }
            }
        }
    }
}

