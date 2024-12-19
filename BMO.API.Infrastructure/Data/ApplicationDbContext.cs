using BMO.API.Core.Entities;
using Microsoft.EntityFrameworkCore;

# nullable disable

namespace BMO.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ScheduledMessage> ScheduledMessages { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<SmsMessage> SmsMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
