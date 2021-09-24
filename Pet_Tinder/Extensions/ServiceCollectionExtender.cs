using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Pet_Tinder.Repositories;
using Pet_Tinder.Repositories.Interfaces;

namespace Pet_Tinder.Extensions
{
	public static class ServiceCollectionExtender
	{
		public static IServiceCollection AddCommonDependenciesDI(this IServiceCollection services)
		{
			services.AddSingleton<ISystemClock, SystemClock>();
			services.AddSingleton<IUserRepository, UserRepository>();

			return services;
		}
	}
}
