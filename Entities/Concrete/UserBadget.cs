using System;
namespace Entities.Concrete
{
    public class UserBadget
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BadgetId { get; set; }
        public Badget Badget { get; set; }

        public int IsActive { get; set; }
    }
}
