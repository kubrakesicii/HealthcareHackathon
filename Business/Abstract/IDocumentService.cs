using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Results;
using Entities.DTOs.Document;

namespace Business.Abstract
{
    public interface IDocumentService
    {
        Task<Result> InsertDocuments();
        Task<DataResult<List<GetDocumentDto>>> GetUserDocuments(int userId);
        Task<Result> DeleteDocument(int id);
    }
}
