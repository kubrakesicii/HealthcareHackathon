using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IDocumentRepository Documents { get; }
        IUserOngoingDonationRepository OngoingDonations { get; }
        IUserCompletedDonationRepository CompletedDonations { get; }
        IMessageRepository Messages { get; }
        bool SaveChanges();
    }
}
