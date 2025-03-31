using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exception
{
    /// <summary>
    /// Excepcion base para todos los errores relacionados con la capa de datos.
    /// </summary>
    public class DataException : Exception 
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DataException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        public DataException(string message) : base(message) 
        {
        }

        /// <summary>   
        /// Inicializa una nueva instancia de la clase <see cref="DataException"/> con un mensaje de error y una excepción.
        /// <param name="message">El mensaje que describe el error.</param>"
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public DataException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Exepcion lanzada cuando ocurre un error de conexion con la base de datos
        /// </summary>
        /// <param name="menssage">El mesanje describe el error de conexion.</param>
        /// <param name="innerException">La excepcion que es la causa del error actual.</param>
        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepcion lanzada cuando ocurre un error de conexion con la base de datos.
    /// </summary>
    public class QueryExecutionException : DataException
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="QueryExecutionException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        public QueryExecutionException(string message) : base(message)
        {
        }
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="QueryExecutionException"/> con un mensaje de error y una excepción.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public QueryExecutionException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepcion lanzada cuando ocurre un conflicto de concurrencia en la base de datos 
    /// </summary>
    public class ConcurrencyException : DataException
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConcurrencyException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        public ConcurrencyException(string message) : base(message)
        {
        }
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ConcurrencyException"/> con un mensaje de error y una excepción.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de concurrencia.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ConcurrencyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DataException"/> con un mensaje de error y una excepción.
    /// </summary>
    /// <param name="message">El mensaje que describe el error de concurrencia.</param>
    /// <param name="innerException">La excepcion que es la causa del error actual.</param>
    public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Execpcion lanzada cuando violan restricciones de integridad en la base de datos.
    /// </summary>
    public class DataIntegrityException : DataException
{
    /// <summary>
    /// Inicializa una nueva instancia de  <see cref="DataIntegrityException"/> con un mensaje de error.
    /// </summary>
    /// <param name="message">El mensaje que describe la violacion de integridad .</param>
    public DataIntegrityException(string message) : base(message)
    {
    }
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="DataIntegrityException"/> con un mensaje de error y una excepción.
    /// </summary>
    /// <param name="message">El mensaje que describe la violacion de integridad .</param>
    /// <param name="innerException">La excepción que es la causa del error actual.</param>
    public DataIntegrityException(string message, System.Exception innerException) : base(message, innerException)
    {
    }
}
