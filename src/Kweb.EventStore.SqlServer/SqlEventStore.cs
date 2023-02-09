using System.Text.Json;
using K.EventStore.SqlServer.Model;
using K.EventStore;
using Microsoft.EntityFrameworkCore;

namespace K.EventStore.SqlServer
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="K.EventStore.IStoreAndRetrieveEvents" />
	public class SqlEventStore : IStoreAndRetrieveEvents
	{
		private readonly IntegrationEventLogContext _ctx;
		private readonly IEnumerable<Type> _availableTypes;
		public SqlEventStore(IntegrationEventLogContext ctx, IEnumerable<IAmAnEventMessage> eventMessages)
		{
			_ctx = ctx;
			_availableTypes = eventMessages.Select(p => p.GetType());
		}
		/// <summary>
		/// Stores for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="aggregateId">The aggregate identifier.</param>
		/// <param name="eventMessage">The event message.</param>
		public async Task StoreFor<TAggregate>(string aggregateId, IAmAnEventMessage eventMessage) where TAggregate : IAggregateRoot
		{
			var messsage = JsonSerializer.Serialize(eventMessage, eventMessage.GetType(), new JsonSerializerOptions(JsonSerializerDefaults.Web));
			var eventLog = new EventLog()
			{
				AggregateId = aggregateId,
				TypeOfAggregate = typeof(TAggregate).FullName!,
				TypeOfEvent = eventMessage.GetType().FullName!,
				Event = messsage,
				TimeStamp = DateTime.UtcNow
			};
			await _ctx.EventLogs.AddAsync(eventLog);
			await _ctx.SaveChangesAsync();
		}
		/// <summary>
		/// Stores for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="eventList">The event list.</param>
		public async Task StoreFor<TAggregate>(List<(string aggregateId, IAmAnEventMessage eventMessage)> eventList) where TAggregate : IAggregateRoot
		{
			var liste2Add = new List<EventLog>();
			foreach (var ev in eventList)
			{
				var messsage = JsonSerializer.Serialize(ev.eventMessage, ev.eventMessage.GetType(), new JsonSerializerOptions(JsonSerializerDefaults.Web)
				{

				});
				var eventLog = new EventLog()
				{
					AggregateId = ev.aggregateId,
					TypeOfAggregate = typeof(TAggregate).FullName!,
					TypeOfEvent = ev.eventMessage.GetType().FullName!,
					Event = messsage,
					TimeStamp = DateTime.UtcNow
				};
				liste2Add.Add(eventLog);
			}
			await _ctx.EventLogs.AddRangeAsync(liste2Add);
			await _ctx.SaveChangesAsync();
		}
		/// <summary>
		/// Retrieves the events for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <param name="aggregateId">The aggregate identifier.</param>
		/// <returns></returns>
		public IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>(string aggregateId) where TAggregate : IAggregateRoot
		{
			var events = _ctx.EventLogs
				.AsNoTracking()
				.Where(ev => ev.AggregateId.Equals(aggregateId)
							 && ev.TypeOfAggregate == typeof(TAggregate).FullName)
				.ToList()

				.Select(p =>
				{
					return (IAmAnEventMessage)JsonSerializer.Deserialize(p.Event,
						_availableTypes.First(t => t.FullName!.Equals(p.TypeOfEvent)), new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
				})
				.AsEnumerable();
			return events;
		}
		/// <summary>
		/// Retrieves the events for.
		/// </summary>
		/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
		/// <returns></returns>
		public IEnumerable<IAmAnEventMessage> RetrieveEventsFor<TAggregate>() where TAggregate : IAggregateRoot
		{
			var events = _ctx.EventLogs
				.AsNoTracking()
				.Where(ev => ev.TypeOfAggregate == typeof(TAggregate).FullName)
				.ToList()

				.Select(p =>
				{
					return (IAmAnEventMessage)JsonSerializer.Deserialize(p.Event,
						_availableTypes.First(t => t.FullName!.Equals(p.TypeOfEvent)), new JsonSerializerOptions(JsonSerializerDefaults.Web))!;
				})
				.AsEnumerable();
			return events;
		}
	}
}
