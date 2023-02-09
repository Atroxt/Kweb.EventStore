namespace K.EventStore.DataAccess
{
	public class InMemoryEventStore : IStoreAndRetrieveEvents
	{
		private readonly IList<EventBag> _events;

		public InMemoryEventStore()
		{
			_events = new List<EventBag>();
		}

		public Task StoreFor<TAggregate>(string aggregateId, IAmAnEventMessage eventMessage)
			where TAggregate : IAggregateRoot
		{
			_events.Add(new EventBag
			{
				AggregateId = aggregateId,
				TypeOfAggregate = typeof(TAggregate),
				TypeOfEvent = eventMessage.GetType(),
				TimeStamp = DateTime.UtcNow,
				Event = eventMessage
			});
			return Task.CompletedTask;
		}

		public IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>(string aggregateId)
			where TAggregate : IAggregateRoot
		{
			return _events
				.Where(eventMessage =>
					eventMessage.AggregateId.Equals(aggregateId)
					&& eventMessage.TypeOfAggregate == typeof(TAggregate))
				.Select(eventBag => eventBag.Event)
				.ToArray();
		}

		public IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>() where TAggregate : IAggregateRoot
		{
			return _events
				.Where(eventMessage =>
					eventMessage.TypeOfAggregate == typeof(TAggregate))
				.Select(eventBag => eventBag.Event)
				.ToArray();
		}

		public Task StoreFor<TAggregate>(List<(string aggregateId, IAmAnEventMessage eventMessage)> eventList)
			where TAggregate : IAggregateRoot
		{
			foreach (var ev in eventList)
			{
				_events.Add(new EventBag
				{
					AggregateId = ev.aggregateId,
					TypeOfAggregate = typeof(TAggregate),
					TypeOfEvent = ev.eventMessage.GetType(),
					TimeStamp = DateTime.UtcNow,
					Event = ev.eventMessage
				});
			}

			return Task.CompletedTask;
		}
	}
}
