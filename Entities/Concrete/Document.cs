using System;
namespace Entities.Concrete
{
    public class Document
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Path { get; set; }

        public DateTime UploadedAt { get; set; }
        public int IsActive { get; set; }
    }
}
