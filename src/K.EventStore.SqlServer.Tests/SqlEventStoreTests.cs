using FluentAssertions;
using K.EventStore.SqlServer.Tests.Aggregate;
using Microsoft.Extensions.DependencyInjection;
using K.EventStore.SqlServer.Tests.Events;
using Microsoft.EntityFrameworkCore;

namespace K.EventStore.SqlServer.Tests
{
	[TestClass]
	public class SqlEventStoreTests
	{
		private IStoreAndRetrieveEvents eventStore = default!;
		private IntegrationEventLogContext dbTestContext = default!;

		[TestInitialize]
		public void Setup()
		{
			var assembly = typeof(SqlEventStoreTests).Assembly;

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSqlEventStore(assembly);
			serviceCollection.AddDbContext<IntegrationEventLogContext>(o =>
			{
				o.EnableSensitiveDataLogging();
				o.UseInMemoryDatabase("TestDb");
			});
			serviceCollection.AddSqlEventStore();
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

		[TestMethod]
		public async Task Can_StoreEventList()
		{
			dbTestContext.EventLogs.RemoveRange(dbTestContext.EventLogs);
			await dbTestContext.SaveChangesAsync();
			var listeEvents = new List<(string aggregateId, IAmAnEventMessage eventMessage)>();
			for (int i = 0; i < 10; i++)
			{
				var id = Guid.NewGuid().ToString();
				var aggregate = new TestAggregate(id);
				var event1 = new TestEvent1();
				listeEvents.Add((aggregate.AggregateId, event1));
			}
			await eventStore.StoreFor<TestAggregate>(listeEvents);

			var result = eventStore.RetrieveEventsFor<TestAggregate>();
			result.Should().HaveCount(10);
		}
	}
}
