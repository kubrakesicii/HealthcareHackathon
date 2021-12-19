using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Message
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentDate { get; set; }
    }
}
