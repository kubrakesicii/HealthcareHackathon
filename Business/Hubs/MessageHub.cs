using Business.Abstract;
using Core.Exceptions;
using DataAccess.Contexts;
using DataAccess.UnitOfWork;
using Entities.Concrete;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class MessageHub : Hub
    {
        private readonly HealthcareContext _context;
        private readonly IMessageService _messageService;

        public MessageHub(HealthcareContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            // Burada kullanıcı- connectioni baglantısı kuran bir data olmalı
            // Eger bu kullanici zaten eklendiyse connid update edilsin
            return base.OnConnectedAsync();
        }

        public async Task JoinRoom(int userId)
        {
            // Kullanıcının telefon bilgisi eksikse bu işlemi hiç yaptırma
            var check = _context.Users.FromSqlRaw($"SELECT * FROM Users WHERE Id = {userId}").FirstOrDefault();
            if (check.Phone == null)
            {
                throw new BadRequestException("PHONE REQUIRED");
            }

            else
            {
                
                // unique room name olusturuldu
                var roomName = Guid.NewGuid().ToString("N");

                Console.WriteLine("Room name : " + roomName);
                // Olusturulan unique chate baslatan kisi direk eklenir.
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }
        }

        public async Task SendMessage(string roomName, string messageTxt, int adId, int userId)
        {
            Message message = new Message
            {
                RoomName = roomName,
                Text = messageTxt,
                SentDate = DateTime.Now,
                SenderId = userId
            };

            // Yeni olusturulan mesaj objesi database insert edilir.
            ////
            await _messageService.InsertMessage(message);

            // İlgili odaya mesaj gönderilir. Bu odaya katılmış olan kullanıcılar socket yardımıyla haberdar olurlar.
            await Clients.Group(roomName).SendAsync("sendMessageToRoom", messageTxt);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
