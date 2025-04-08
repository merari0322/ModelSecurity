using Data;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;



namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los documentos del sistema.
    /// </summary>
    public class DocumentBusiness
    {
        private readonly DocumentData _documentData;
        private readonly ILogger _logger;

        public DocumentBusiness(DocumentData documentData, ILogger logger)
        {
            _documentData = documentData;
            _logger = logger;
        }

        // Método para obtener todos los documentos como DTOs
        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync()
        {
            try
            {
                var documents = await _documentData.GetAllAsync();
                return MapToDocumentDTOList(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los documentos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de documentos", ex);
            }
        }

        // Método para obtener un documento por ID como DTO
        public async Task<DocumentDTO> GetDocumentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un documento con ID inválido: {DocumentId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del documento debe ser mayor que cero");
            }

            try
            {
                var document = await _documentData.GetByIdAsync(id);
                if (document == null)
                {
                    _logger.LogInformation("No se encontró ningún documento con ID: {DocumentId}", id);
                    throw new EntityNotFoundException("Document", id);
                }

                return MapToDocumentDTO(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el documento con ID: {DocumentId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el documento con ID {id}", ex);
            }
        }

        // Método para crear un documento desde un DTO
        public async Task<DocumentDTO> CreateDocumentAsync(DocumentDTO documentDto)
        {
            try
            {
                ValidateDocument(documentDto);

                var document = MapToDocument(documentDto);

                var documentCreated = await _documentData.CreateAsync(document);

                return MapToDocumentDTO(documentCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo documento: {DocumentTitle}", documentDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el documento", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateDocument(DocumentDTO documentDto)
        {
            if (documentDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto documento no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(documentDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un documento con Title vacío");
                throw new Utilities.Exceptions.ValidationException("Title", "El Title del documento es obligatorio");
            }
        }

        //Metodo para mapear el Document a DocumentDTO
        private DocumentDTO MapToDocumentDTO(Document document)
        {
            return new DocumentDTO
            {
                Id = document.Id,
                Url = document.Url,
                Name = document.Name,
            };
        }

        //Metodo para mapear el DocumentDTO a Document
        private Document MapToDocument(DocumentDTO documentDto)
        {
            return new Document
            {
                Id = documentDto.Id,
                Url = documentDto.Url,
                Name = documentDto.Name,
            };
        }

        //Metodo para mapear una lista de Document a una lista de DocumentDTO
        private List<DocumentDTO> MapToDocumentDTOList(IEnumerable<Document> documents)
        {
            var documentsDTO = new List<DocumentDTO>();
            foreach (var document in documents)
            {
                documentsDTO.Add(MapToDocumentDTO(document));
            }
            return documentsDTO;
        }
    }
}
