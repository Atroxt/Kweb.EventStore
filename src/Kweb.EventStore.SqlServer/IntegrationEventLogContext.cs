using K.EventStore.SqlServer.Model;
using Microsoft.EntityFrameworkCore;

namespace K.EventStore.SqlServer
{
	public class IntegrationEventLogContext : DbContext
	{
		public IntegrationEventLogContext() { }

		public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options) { }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Fcs.Core.EventStore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
			}
			base.OnConfiguring(optionsBuilder);
		}

		public virtual DbSet<EventLog> EventLogs { get; set; } = default!;
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			CreateEventLog(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		private void CreateEventLog(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EventLog>(e =>
			{
				e.ToTable("IntegrationEvents");
				e.HasKey(p => p.EventLogId);
				e.Property(p => p.AggregateId).IsRequired().HasMaxLength(70);
				e.Property(p => p.TypeOfAggregate).IsRequired();
				e.Property(p => p.TypeOfEvent).IsRequired();
				e.Property(p => p.TimeStamp).IsRequired().HasDefaultValueSql("GETDATE()");
				e.Property(p => p.Event);
			});
		}
	}
}