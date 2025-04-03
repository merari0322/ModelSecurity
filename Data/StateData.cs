using Entity.Contexs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class StateData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StateData> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public StateData(ApplicationDbContext context, ILogger<StateData> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            return await _context.Set<State>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<State?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<State>().FindAsync(id);
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
        ///<param name="state">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<State> CreateAsync(State state)
        {
            try
            {
                await _context.Set<State>().AddAsync(state);
                await _context.SaveChangesAsync();
                return state;
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
        ///<param name="state">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(State state)
        {
            try
            {
                _context.Set<State>().Update(state);
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
                var state = await _context.Set<State>().FindAsync(id);
                if (state == null)
                    return false;

                _context.Set<State>().Remove(state);
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
