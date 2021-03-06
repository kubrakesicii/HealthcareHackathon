using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.UnitOfWork;
using Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class DocumentService : IDocumentService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authService;

        public DocumentService(IUnitOfWork unitOfWork, IAuthenticationService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<Result> InsertDocuments()
        {
            // Aynı anda bir veya daha fazla document eklenebilir. Raporlar kisiye baglı eklenir
            return await _unitOfWork.Documents.InsertDocuments(_authService.Id);
        }

        public async Task<DataResult<List<GetDocumentDto>>> GetUserDocuments(int userId)
        {
            return await _unitOfWork.Documents.GetUserDocuments(userId);
        }

        public async Task<Result> DeleteDocument(int id)
        {
            return await _unitOfWork.Documents.DeleteDocument(id);
        }
    }
}
