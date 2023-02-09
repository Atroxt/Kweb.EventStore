using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace K.EventStore.SqlServer
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddSqlEventStore(this IServiceCollection services,
			string sqlConnectionString, params Assembly[] assembliesToScan)
		{
			services.AddSqlEventStore(assembliesToScan);
			services.AddSqlEventStoreDbContext(sqlConnectionString);
			services.AddSqlEventStore();
			return services;
		}
		public static IServiceCollection AddSqlEventStore(this IServiceCollection services, params Assembly[] assembliesToScan)
		{
			assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();
			foreach (var type in assembliesToScan
						 .SelectMany(a => a.DefinedTypes)
						 .Where(t => !t.IsAbstract && !t.IsInterface)
						 .Where(t => typeof(IAmAnEventMessage).IsAssignableFrom(t))
					)
			{
				services.AddTransient(typeof(IAmAnEventMessage), type);
			}
			return services;
		}
		public static IServiceCollection AddSqlEventStore(this IServiceCollection services)
		{
			services.AddTransient<IStoreAndRetrieveEvents, SqlEventStore>();
			return services;
		}
		public static IServiceCollection AddSqlEventStoreDbContext(this IServiceCollection services, string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new NullReferenceException("[AddFcsSqlEventStore*] sql Connection string is empty!");
			}
			var sqlConnectionString = connectionString;
			services.AddDbContext<IntegrationEventLogContext>(o =>
			{
				o.UseSqlServer(sqlConnectionString);
			});
			return services;
		}

		public static void MigrateDatabase(this IServiceCollection services)
		{
			try
			{
				var sp = services.BuildServiceProvider();
				var store = sp.GetRequiredService<IntegrationEventLogContext>();
				store.Database.Migrate();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
