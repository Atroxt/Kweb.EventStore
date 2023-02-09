namespace K.EventStore;
/// <summary>
/// define a Aggregation root
/// </summary>
public interface IAggregateRoot
{
	/// <summary>
	/// Gets the aggregate identifier.
	/// </summary>
	/// <value>
	/// The aggregate identifier.
	/// </value>
	string AggregateId { get; }
}