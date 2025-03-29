using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace Entity.Contex
{
    ///<summary>
    ///Representa el  contexto de la base de datos de la aplicacion, proporcionanado configuracion y metodos
    ///para la representacion de entidades y consultas personalizadas con Dapper 
    ///<summary/>
    class ApplicationDbContext : DbContext
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
        ///<summary>
        ///Configura opciones adicionales del contexto, como el registro de datos sensibles.
        ///</summary>
        ///<param name="optionsBuilder">Constructor de modelo de base de datos</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        ///<summary>
        ///Configura opciones adicionles del contesxto, como el registro de dats sensibles.
        ///</summary>
        ///<param name="optionsBuilder">Constructor de opciones de configuracion del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            //Otras configuraciones adicionales pueden ir aqui
        }

        ///<summary>
        ///Configura convenciones de tipos de datos, estableciendo la precision por defecto de los valores decimales.
        ///</summary>
        ///<param name="configurationBuilder">Constructor de configuracion de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HasPrecision(18, 2);
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
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommans(this, text, parameters, timeout, type, CancellationTRoken.None);
            var connection = this.Database.GetDbConnection();
        }

        ///<summary>
        ///ejhecuta una consulta SQL utilizando Dapper y devuelve un solo resultado o el valor predeterminado si no hay resultados.
        ///</summary>
        ///<typeparam name="T">Tipo de datos de retorno.</typeparam>
        ///<param name="text">Consulta SQL a ejecutar.</param>
        ///<param name="parameters">Parametros opcionales de la consulta.</param>
        ///<param name="type">Tipo opcional de comando SQL.</param>
        ///<returns>Un objeto del tipo especificado o su valor prederterminado.</returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string text, object parameters = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters, null, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        ///<summary>
        ///Metodo interno para garantizar la auditoria d elos cambios en la entidades.
        ///</summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
        }

        ///<summary>
        ///Estructura para ejecutar comandos SQL con Dapper en Entity Franmework Core.
        ///</summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            ///<summary>
            ///Constructor del comando Dapper.
            ///</summary>
            ///<param name="context">Contexto de l a base de datos.</param>
            ///<param name="text">Consulta SQL.</param>
            ///<param name="parameters">parametro opcionales.</param>
            ///<param name="timeout">Tiempo de espera opcional.</param>
            ///<param name="type">tipo de comando SQL opcional.</param>
            ///<param name="ct">Token de cancelacion.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters. int timeout, CommandType type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                AssemblyDefinition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    CancellationToken: ct
                    );
            }
        }
        ///<summary>
        ///define  los parametros del comando SQL,
        ///</summary>
        public CommandDefinition Definition { get; }

        ///<summary>
        ///Metodo para liberar los recursos.
        ///</summary>
        public void Dispose()
        {
        }
    } 
}
