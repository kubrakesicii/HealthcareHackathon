using System;
using DataAccess.Configurations;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
        public class HealthcareContext : DbContext
        {
            public HealthcareContext()
            {
            }

            public HealthcareContext(DbContextOptions<HealthcareContext> options) : base(options)
            {

            }

            public DbSet<User> Users { get; set; }
            public DbSet<Donation> Donations { get; set; }
            public DbSet<UserCompletedDonation> UserCompletedDonations { get; set; }
            public DbSet<UserOngoingDonation> UserOngoingDonations { get; set; }
            public DbSet<Document> Documents { get; set; }
            public DbSet<District> Districts { get; set; }
            public DbSet<City> Cities { get; set; }
            public DbSet<Country> Countries { get; set; }
            public DbSet<Badget> Badgets { get; set; }
            public DbSet<UserBadget> UserBadgets { get; set; }
            public DbSet<Message> Messages { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                if (!options.IsConfigured)
                    options.UseSqlServer("Server=127.0.0.1,1433; Database=HealthcareDb; User=sa; Password = AyremX.123", x => x.MigrationsAssembly("DataAccess"));
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new UserConfig());
                modelBuilder.ApplyConfiguration(new DonationConfig());
                modelBuilder.ApplyConfiguration(new DocumentConfig());
                modelBuilder.ApplyConfiguration(new BadgetConfig());
            }
    }
}
