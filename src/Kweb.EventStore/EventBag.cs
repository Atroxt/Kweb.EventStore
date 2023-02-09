namespace K.EventStore;
/// <summary>
/// EventBag
/// </summary>
public class EventBag
{
	public string AggregateId { get; set; } = default!;
	public Type TypeOfAggregate { get; set; } = default!;
	public Type TypeOfEvent { get; set; } = default!;
	public IAmAnEventMessage Event { get; set; } = default!;
	public DateTime TimeStamp { get; set; }
}