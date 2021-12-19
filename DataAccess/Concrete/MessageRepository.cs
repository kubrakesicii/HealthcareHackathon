using Core.Results;
using DataAccess.Abstract;
using DataAccess.Contexts;
using DataAccess.Repositories;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(HealthcareContext context) : base(context)
        {
        }
        public async Task<DataResult<List<Message>>> GetMessagesOfUser(int userId)
        {
            return new DataResult<List<Message>>(await _context.Messages.Where(x => x.ReceiverId == userId).Select(x => new Message
            {
                Id = x.Id,
                SenderId = x.SenderId,
                ReceiverId = x.ReceiverId,
                Text = x.Text,
                SentDate = x.SentDate,
                RoomName = x.RoomName
            }).ToListAsync(), true);
        }
    }
}
