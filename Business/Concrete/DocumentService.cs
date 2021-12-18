using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class DocumentService : IDocumentService
    {
        public IDocumentRepository _documentRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentService(IDocumentRepository documentRepo,IHttpContextAccessor httpContextAccessor)
        {
            _documentRepo = documentRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> InsertDocuments(int userId, List<string> docPaths)
        {
            await _documentRepo.InsertDocuments(userId, docPaths);
            return new Result(true);
        }
    }
}
