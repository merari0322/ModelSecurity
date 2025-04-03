using Entity.Contexs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
     public class ExperiencieLineThematicData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExperiencieLineThematicData> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public ExperiencieLineThematicData(ApplicationDbContext context, ILogger<ExperiencieLineThematicData> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<ExperienciaLineThematic>> GetAllAsync()
        {
            return await _context.Set<ExperienciaLineThematic>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<ExperienciaLineThematic?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ExperienciaLineThematic>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ron con ID {RolId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo rol en la base de datos.
        ///</summary>
        ///<param name="ExperienciaLineThematic>Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<ExperienciaLineThematic> CreateAsync(ExperienciaLineThematic ExperienciaLineThematic)
        {
            try
            {
                await _context.Set<ExperienciaLineThematic>().AddAsync(ExperienciaLineThematic);
                await _context.SaveChangesAsync();
                return ExperienciaLineThematic;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol:{ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Actualiza un rol existente en la base de datos.
        ///</summary>
        ///<param name="ExperienciaLineThematic">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(ExperienciaLineThematic ExperienciaLineThematic)
        {
            try
            {
                _context.Set<ExperienciaLineThematic>().Update(ExperienciaLineThematic);
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
        ///Elimina un rol de la base de datos.
        ///</summary>
        ///<param name="id">Identificador único del rol a eliminar</param>
        ///<returns>True si la eliminación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var ExperienciaLineThematic = await _context.Set<ExperienciaLineThematic>().FindAsync(id);
                if (ExperienciaLineThematic == null)
                    return false;

                _context.Set<ExperienciaLineThematic>().Remove(ExperienciaLineThematic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Error al eliminar el rol: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
