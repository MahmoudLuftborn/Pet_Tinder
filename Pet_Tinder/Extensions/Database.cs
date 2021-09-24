using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

using System;

namespace Pet_Tinder.Extensions
{
	public static class Database
	{
		public static IServiceCollection AddMongoDBDependencies(this IServiceCollection services)
		{

			services.AddSingleton<IMongoClient, MongoClient>(provider =>
			{
				var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
				return new MongoClient(connectionString);
			});


			services.AddSingleton(provider =>
			{
				var configuration = provider.GetService<IConfiguration>();
				var databaseName = configuration.GetSection("Database").GetValue<string>("Name");
				var client = (IMongoClient)provider.GetService(typeof(IMongoClient));
				return client.GetDatabase(databaseName);
			});


			return services;
		}
	}
}
