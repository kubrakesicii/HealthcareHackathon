using System;
namespace Entities.DTOs.Document
{
    public class GetDocumentDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
