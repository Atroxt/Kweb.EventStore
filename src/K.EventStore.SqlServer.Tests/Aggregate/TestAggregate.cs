namespace K.EventStore.SqlServer.Tests.Aggregate
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
