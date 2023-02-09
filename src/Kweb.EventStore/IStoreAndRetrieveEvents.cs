namespace K.EventStore
{
	/// <summary>
	/// Responsible for store and retrieve Events.
	/// </summary>
	public interface IStoreAndRetrieveEvents
	{
		/// <summary>
		/// Stores for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="aggregateId">The aggregate identifier.</param>
		/// <param name="eventMessage">The event message.</param>
		/// <returns></returns>
		Task StoreFor<TAggregate>(string aggregateId, IAmAnEventMessage eventMessage) where TAggregate : IAggregateRoot;
		/// <summary>
		/// Retrieves the events for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="aggregateId">The aggregate identifier.</param>
		/// <returns></returns>
		IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>(string aggregateId) where TAggregate : IAggregateRoot;
		/// <summary>
		/// Retrieves the events for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <returns></returns>
		IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>() where TAggregate : IAggregateRoot;
		/// <summary>
		/// Stores for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="eventList">The event list.</param>
		/// <returns></returns>
		Task StoreFor<TAggregate>(List<(string aggregateId, IAmAnEventMessage eventMessage)> eventList) where TAggregate : IAggregateRoot;
	}
}