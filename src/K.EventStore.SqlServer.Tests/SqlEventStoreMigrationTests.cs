using FluentAssertions;
using K.EventStore.SqlServer.Tests.Aggregate;
using K.EventStore.SqlServer.Tests.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace K.EventStore.SqlServer.Tests
{
	[Ignore("only local check with sqlConnection")]
	[TestClass]
	public class SqlEventStoreMigrationTests
	{
		private IStoreAndRetrieveEvents eventStore = default!;
		private IntegrationEventLogContext dbTestContext = default!;
		[TestInitialize]
		public void Setup()
		{
			var assembly = typeof(SqlEventStoreMigrationTests).Assembly;

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSqlEventStore(assembly);
			serviceCollection.AddDbContext<IntegrationEventLogContext>(o =>
			{
				o.EnableSensitiveDataLogging();
				o.UseSqlServer(
					@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EventStoreTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
				//o.UseInMemoryDatabase("TestDb");
			});
			serviceCollection.AddSqlEventStore();
			serviceCollection.MigrateDatabase();
			var sp = serviceCollection.BuildServiceProvider();

			dbTestContext = sp.GetRequiredService<IntegrationEventLogContext>();
			eventStore = sp.GetRequiredService<IStoreAndRetrieveEvents>();
		}

		[TestMethod]
		public async Task Can_StoreEvent()
		{
			var id = Guid.NewGuid().ToString();
			var aggregate = new TestAggregate(id);
			var event1 = new TestEvent1() { DateToCheck = DateTime.UtcNow };
			var event2 = new TestEvent2();

			await eventStore.StoreFor<TestAggregate>(aggregate.AggregateId, event1);
			await eventStore.StoreFor<TestAggregate>(aggregate.AggregateId, event2);

			var result = eventStore.RetrieveEventsFor<TestAggregate>(aggregate.AggregateId);
			result.Should().HaveCount(2);
		}
	}
}