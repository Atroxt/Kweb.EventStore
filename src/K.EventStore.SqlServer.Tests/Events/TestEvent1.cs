namespace K.EventStore.SqlServer.Tests.Events;

public class TestEvent1 : IAmAnEventMessage
{
	public DateTime DateToCheck { get; set; }
}