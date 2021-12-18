using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Results;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Document;

namespace DataAccess.Abstract
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<Result> InsertDocuments(int userId);
        Task<List<GetDocumentDto>> GetUserDocuments(int userId);
    }
}
