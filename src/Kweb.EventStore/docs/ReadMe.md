# K.EventStore
base definitions to Store and retrieve events

All events must implement the _IAmAnEventMessage_ interface.
Your Aggregate must be derived from IAggregateRoot

Your EventStore must be implement the _IStoreAndRetrieveEvents_ interface.