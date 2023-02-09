# K.EventStore.SqlServer
this packages is to persist the events in an SqlTable.

## How to use

implement in you ServiceCollection

```c#
Assembly[] assemblysToScan;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlEventStore("SQLCONNECTIONSTRING",assemblysToScan);
```

If you need to add the migration to your Database you can use 
the extensionMethod _MigrateDatabase_ to build up an Table to your Database.

```c#
builder.Services.MigrateDatabase();
```
But first of all the _AddSqlEventStore_ has to be added.

