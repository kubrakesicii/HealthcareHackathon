using System;
namespace Entities.Concrete
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int IsActive { get; set; } = 1;
    }
}
