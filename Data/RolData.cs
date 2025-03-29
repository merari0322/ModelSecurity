using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Repositorio encargado de la gestion de la entidad Rol en la base de datos.
    /// </summary>
    class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly Ilogger _logger;

        /// <summary>
        /// Contructor que recibe el contesto de base de datos.
        /// </summary>
        /// <param name="context">Instacia de <see cref="ApplicationDbContext"</param>
        public RolData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        ///<summary>
        ///Obtinen todos los roles de la base de datos.
        ///</summary>
        ///<returns>Lista de roles.</returns>
        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Set<Rol>().ToListAsync();
        }
        public async Task<Rol> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rol>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RolId}", id);
                throw;//Re-lanza la execpcion para que sea manejada en capas supueriores 
            }
        }

        ///<summary>
        ///Crea un nuevo rol en la base de datos
        ///</summary>
        ///<param name="rol">Instancia de rol a crear.</param>
        ///<returns>El rol no creado.</returns>
        public async Task<Rol> CreateAsync(Rol rol)
        {
            try
            {
                await _context.Set<Rol>().Add(rol);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol: {ex.Message}");
                throw;
            }
        }
        ///<summary>
        ///Actualiza un rol en la base de datos
        ///</summary>
        ///<param name="rol">Objeto con la informacion actualizada.</param>"
        ///<returns>True si la operacion fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                _context.Set<Rol>().Update(Rol rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                throw;
            }
        }
        ///<summary>
        ///Elimina un rol de la base de dtaos.
        ///</summary>
        ///<param name="id">Identificador unico deñl rol eliminar.</param>
        ///<returns>True si la eliminacion fue existosa, False en caso contrario</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rol = await _context.Set<Rol>().FindAsync(id);

                if (rol == null)
                    return false;
                _context.Set<Rol>().Remove(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el rol: {ex.Message}");
                return false;
            }

        }
    }
}
