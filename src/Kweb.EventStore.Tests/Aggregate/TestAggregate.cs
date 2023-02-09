namespace K.EventStore.Tests.Aggregate
{
	internal class TestAggregate : IAggregateRoot
	{
		public TestAggregate(string aggregateId)
		{
			AggregateId = aggregateId;
		}
		public string AggregateId { get; }
	}
}
