namespace K.EventStore.Tests.Events;

public class TestEvent1 : IAmAnEventMessage
{
	public DateTime DateToCheck { get; set; }
}