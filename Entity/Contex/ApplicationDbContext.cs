using System.Data;
using Dapper;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Entity.Contexs
{
    ///<summary>
    ///Representa el  contexto de la base de datos de la aplicacion, proporcionanado configuracion y metodos
    ///para la representacion de entidades y consultas personalizadas con Dapper 
    ///<summary/>
    public class ApplicationDbContext : DbContext
    {
        ///<summary> 
        ///configuracion de la aplicacion 
        ///</summary>
        protected readonly IConfiguration _configuration;

        ///<summary>
        ///Contructor del contexto de la base de datos
        ///</summary>
        ///<param name="options">Opciones de configuracion para el contexto de base de datos.</parm>
        ///<param name="configuration">Instancia de IConfuration para acceder a la configuracion de la aplicacion</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        ///<summary> 
        ///Configura los modelos de la base de datos aplicando configuraciones desde ensamblados.
        ///</summary>
        ///<param name="modelBuilder">Contructor del modelo de base de datos.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Otras configuraciones adicionales pueden ir aquí
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        ///<summary>
        ///Guarda los cambios en la base de datos, asegurando la auditoria antes de persistirlos datos.
        ///</summary>
        ///<returns>Numero de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        ///<summary>
        ///Guarda los cambios en la base de datos de manera asincrona, asegurando la auditoria antes de la persistencia.
        ///</summary>
        ///<param name="acceptAllChangesOnSuccess">Indica si deben aceptar todos los cambios en caso de exito.</param>
        ///<param name="cancellationToken">Token de cancelacion para abortar la operacion.</param>
        ///<returns>nuemro de filas afectadas de forma asincrona.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        ///<summary>
        ///Ejecuta una consulta SQL utilizando Dapper y devuelve una coleccion de resultados de tipo generico.
        ///</summary>
        ///<typeparam name="T">tipo de los datos de retorno.</typeparam>
        ///<param name="text">Consulta SQL a ejecutar.</param>
        ///<param name="parameters">Parametros de la consulta SQL.</param>
        ///<param name="timeout">Tiemmpo de espera opcional para la consulta.</param>
        ///<param name="tipe">Tipo opcional de comandos SQL.</param>
        ///<returns>Una coleccion de objetos del tipo especificado.</returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string text, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            /// <summary>
            /// Constructor del comando Dapper.
            /// </summary>
            /// <param name="context">Contexto de la base de datos.</param>
            /// <param name="text">Consulta SQL.</param>
            /// <param name="parameters">Parámetros opcionales.</param>
            /// <param name="timeout">Tiempo de espera opcional.</param>
            /// <param name="type">Tipo de comando SQL opcional.</param>
            /// <param name="ct">Token de cancelación.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            /// <summary>
            /// Define los parámetros del comando SQL.
            /// </summary>
            public CommandDefinition Definition { get; }

            /// <summary>
            /// Método para liberar los recursos.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}