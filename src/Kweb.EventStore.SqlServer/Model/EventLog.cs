namespace K.EventStore.SqlServer.Model
{
	public class EventLog
	{
		public Guid EventLogId { get; set; } = Guid.NewGuid();
		public string AggregateId { get; set; } = default!;
		public string TypeOfAggregate { get; set; } = default!;
		public string TypeOfEvent { get; set; } = default!;
		public string Event { get; set; } = default!;
		public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
	}
}
