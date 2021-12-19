using Business.Abstract;
using Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] InsertDocumentDto documents)
        {
            return Ok(await _documentService.InsertDocuments());
        }

        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery, Required] int userId)
        {
            return Ok(await _documentService.GetUserDocuments(userId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok(await _documentService.DeleteDocument(id));
        }
    }
}
