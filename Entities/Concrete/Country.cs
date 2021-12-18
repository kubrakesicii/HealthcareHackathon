using System;
namespace Entities.Concrete
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }

        public int IsActive { get; set; }
    }
}
