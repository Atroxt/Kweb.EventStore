using FluentAssertions;
using K.EventStore.DataAccess;
using K.EventStore.Tests.Aggregate;
using K.EventStore.Tests.Events;

namespace K.EventStore.Tests
{
	[TestClass]
	public class EventStoreTests
	{
		private IStoreAndRetrieveEvents eventStore = default!;
		private readonly string _aggregateId = Guid.Parse("12ab1f7f-d37d-4436-b8f1-e2ee8ce2119d").ToString();
		[TestInitialize]
		public void Setup()
		{
			eventStore = new InMemoryEventStore();
		}
		[TestMethod]
		public void Can_Store_And_Retrieve_Event()
		{
			var id = _aggregateId;
			var aggregate = new TestAggregate(id);
			var event1 = new TestEvent1() { DateToCheck = DateTime.UtcNow };
			var event2 = new TestEvent2();
			eventStore.StoreFor<TestAggregate>(aggregate.AggregateId, event1);
			eventStore.StoreFor<TestAggregate>(aggregate.AggregateId, event2);
			var result = eventStore.RetrieveEventsFor<TestAggregate>(aggregate.AggregateId);
			result.Should().HaveCount(2);
		}

	}
}