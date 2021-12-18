using System;
namespace Entities.Concrete
{
    public class Donation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int IsActive { get; set; }
    }
}
