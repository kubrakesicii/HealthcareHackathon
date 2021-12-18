using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete
{
    public class DocumentRepository : GenericRepository<Document> , IDocumentRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GetDocumentDto>> GetUserDocuments(int userId)
        {
            return await _context.Documents.Where(x => x.UserId == userId).Select(x => new GetDocumentDto
            {
                Id = x.Id,
                Path = x.Path
            }).ToListAsync();
        }

        public async Task<Result> InsertDocuments(int userId)
        {
            List<string> docPaths = _httpContextAccessor.HttpContext.Items["DocPaths"] as List<string>;

            foreach (var doc in docPaths){
                await InsertAsync(new Document
                {
                    UserId = userId,
                    Path = doc,
                    UploadedAt = DateTime.Now,
                    IsActive = 1
                });
            }

            return new Result(true);
        }
    }
}
