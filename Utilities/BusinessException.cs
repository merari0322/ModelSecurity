using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{ 
//<summary>
///Excepcion base ´para las todos  los errores relacionados con la capa de negocio.
///</summary>
}
public class BusinessException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="BusinessException"/> con un mensaje de error.
    /// </summary>
    /// <param name="message">Mensaje de error que describe el error.</param>"
    public BusinessException(string message) : base(message)
    {
    }

    ///<summary> 
    ///Inicializa una nueva instancia de <see cref="BusinessException"/> con un mensaje de error y una excepcion interna.
    ///</summary>
    ///<param name="menssage">El mensaje que describee el error.</param>
    ///<param name="innerException">La excepcion que es la causa del error actual.</param>
    public BusinessException(string message, Exception innerException) : base(message, innerException)
    {
    }

    ///<summary>
    ///Excepcion lazada cuando no se encuentra una entidad en el sistema.
    ///</summary>
    public class NotFoundException : BusinessException
    {
        ///<summary>
        ///Tipo de entidad que no se encontró.
        ///</summary>
        public string EntityType { get; }

        //summary>
        ///Indentificador de la entidad buscada
        ///</summary>
        public object EntityId { get; }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="NotFoundException"/> con un mensaje personalizado.
        ///</summary>
        ///<param name="menssage">El mensaje que describe el error.</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="NotFoundException"/> con informacion detallada,
        ///</summary>
        ///<param name="entityType">Tipo de entidad que o se encontre </param>
        ///
    }
}
