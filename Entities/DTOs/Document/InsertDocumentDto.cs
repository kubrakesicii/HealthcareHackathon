using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Entities.DTOs.Document
{
    public class InsertDocumentDto
    {
        public List<IFormFile> Documents{ get; set; }
    }

}
