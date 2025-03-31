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
        ///<param name="entityId">identificador de la entidad buscada</param>
        public EntityNotFoundException(string entityType, object entityId)
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="NotFoundException"/> con informacion detallada.
        ///</summary>
        ///<param name="entityType">Tipo de entidad que no se encontro.</param>
        ///<paran name="entityId">Identificador de la entidad buscada.</param>
        ///<param name="innerexception">La excecion que es la causa ddel error actual</param>
        public EntityNotFoundException(string entityType, object entityId, Exception innerException) : base($"Entity '{entityType}' with id '{entityId}' not found.", innerException)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
    ///<summary>
    ///Excepcion lanzada cuando los datos proporcionados no cumplen con laas reglas de validacion.
    ///</summary>
    public class ValidationException : BusinessException
    {
        ///<summary>
        ///campo que no cumple con validacion.
        ///</summary>
        public string PropertyName { get; }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="ValidationException"/> con informacion del campo invalido.
        ///</summary>
        ///<param name="propertyName">Nombre de la propiedad que fallo la validacion,</param>
        ///<param name="message">Mensaje que describe el error de validaacion.</param>
        public ValidationException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        ///<summary>
        ///Inicializa un nueva instancia de <see cref="ValidationException"/> con informacion detallada.
        ///</summary>
        ///<param name="message">El mensage describe el error.</param>
        ///<param name="innerException">La excepcion que es la causa del error actual.</param>
        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    ///<summary>
    ///Exception lanzada cuando se intenta realizar una operacion que viola reglas de negocios
    ///</summary>
    public class BusinessRuleException : BusinessException
    {
        ///<summary>
        ///codigo que identifica la regla de negocio violada.
        public string Rulecode { get; }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="BusinessRuleException"/> con un codigo de regla
        ///</summary>
        ///<param name="message">El mensaje que describe la violacion de la regla de negocio.</param>
        public BusinessRuleException(string message) : base(message)
        {
        }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="BusinessRuleException"/> con un codigo de regla .
        ///</summary>
        ///<param name="ruleCode">Codigo que identifica la regla de negocio violada.</param>
        ///<param name="message">El mesage que describe la violacion.</param>
        public BusinessRuleException(string ruleCode, string message) : base($"Violacion de regla de negocio '{ruleCode}': {message}")
        {
            RuleCode = ruleCode;
        }

        ///<summary>
        ///Inicializa una nueva instancia de <see cref="BusinessRuleException"/> con informacion detallada.
        ///</summary>
        ///<param name="message">El mensaje que descrie el error.</param>
        ///<param name="innerException">La excepcion que es la causa del error actual.</param>
        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        ///<summary>
        ///Excepcion lanzada cuando se intenta acceder a un recurso sin los permisos adecuados.
        ///</summary>
        public class UnauthorizedAccessBusinessException : BusinessException
        {
            ///<summary>
            ///Recurso al que se inteno acceder.
            ///</summary>
            public string Resource { get; }

            ///<summary>
            ///Tipo de operacion que se intento realizar.
            /// </summary>
            public string Operation { get; }

            ///<summary>
            ///Inicializa una nueva instancia de <see cref="UnauthorizedAccessBusinessException"/> con un mensanje de error.
            ///</summary>
            ///<param name="message">El mensaje que describe el errorde autorizacio. </param>
            public UnauthorizedAccessBusinessException(string message) : base(message)
            {
            }

            ///<summary>
            ///Inicializa una nueva instancia de <see cref="UnauthorizedAccessBusinessException"/> con informacion detallada.
            ///</summary>
            ///<param name="resource">Recurso al que se intento acceder.</param>
            ///<param name="operation">Tipo de operacion que se intento realizar.</param>   
            public UnauthorizedAccessBusinessException(string resource, string operation) : base($"Aceso no autorizado al recurso '{resource}' para la operacion '{operation}'.")
            {
                Resource = resource;
                Operation = operation;
            }
            ///<summary>
            ///Inicializa una nueva instacia de <see cref="UnauthorizedAccessBusinessException"/> con informacion detallada.
            ///</summary>
            ///<param name="mesage">El mensaje que describe el error.</param>
            ///<param name="innerException">La excepcion que es la causa del error actual.</param>
            public UnauthorizedAccessBusinessException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
        ///<summary>
        ///Exccepcion lanzada cuando ocurre un error al interectuar con recursos externos como bases de datos o servicios.
        ///</summary>
        public class ExternalServiceException : BusinessException
        {
            ///<summary>
            ///Nombre del servicio que genero el error.
            ///</summary>
            public string ServiceName { get; }

            ///<summary>
            ///Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con un mensaje de error.
            ///</summary>
            ///<param name="message">El mensaje que describe el error del servicio externo.>/
            public ExternalServiceException(string message) : base(message)
            {
            }

            ///<summary>
            ///Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con informacion detallada.
            ///</summary>
            ///<param name="serviceName">Nombre del servicio externo.</param> 
            ///<param name="message">El mensaje que describe el error.</param>
            public ExternalServiceException(string serviceName, string message) : base($"Error en el servicio externo '{serviceName}': {message}")
            {
                ServiceName = serviceName;
            }

            ///<summary>
            ///Inicializa una nueva instancia de <see cref="ExternalServiceException"/> con informacion detallada.
            ///</summary>
            ///<param name="ServiceName">Nombre del servicio externo.</param>
            ///<param name="message">El mensaje que describe el error.</param>
            ///<param name="innerException">La excepcion que es la causa del error actual.</param>
            public ExternalServiceException(string serviceName, string message, Exception innerException) : base($"Error en el servicio externo '{serviceName}': {message}", innerException)
            {
                ServiceName = serviceName;
            }
        }
    }
}

