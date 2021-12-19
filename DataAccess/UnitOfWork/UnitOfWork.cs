using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly HealthcareContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationRepository _authRepo;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserOngoingDonationRepository _ongoingDonationRepository;
        private readonly IUserCompletedDonationRepository _completedDonationRepository;
        private readonly IMessageRepository _messageRepository;

        public UnitOfWork(HealthcareContext context, IHttpContextAccessor httpContextAccessor, IAuthenticationRepository authRepo)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _authRepo = authRepo;
        }

        public IUserRepository Users => _userRepository ?? new UserRepository(_context, _httpContextAccessor);

        public IDocumentRepository Documents => _documentRepository ?? new DocumentRepository(_context, _httpContextAccessor);

        public IUserOngoingDonationRepository OngoingDonations => _ongoingDonationRepository ?? new UserOngoingDonationRepository(_context);

        public IUserCompletedDonationRepository CompletedDonations => _completedDonationRepository ?? new UserCompletedDonationRepository(_context, _authRepo);

        public IMessageRepository Messages => _messageRepository ?? new MessageRepository(_context);

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public bool SaveChanges()
        {
            bool returnValue = true;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return returnValue;
        }

        public void Dispose()
        {
            Dispose(true);
        }


        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
            } 

            _disposedValue = true;
        }
    }
}
