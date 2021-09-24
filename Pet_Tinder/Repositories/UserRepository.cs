using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Internal;

using MongoDB.Driver;

using Pet_Tinder.Models.Domain;
using Pet_Tinder.Models.DTO.Search;
using Pet_Tinder.Repositories.Interfaces;

using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Pet_Tinder.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{

		public UserRepository(ILogger<UserRepository> logger,
								IMongoDatabase database,
								ISystemClock systemClock)
								: base(logger, database, systemClock)
		{
		}

		public async Task<User> GetByEmailAsync(string email)
		{
			var query = Collection.Find(u => u.Email == email);
			return await query.FirstOrDefaultAsync();
		}

		public async Task<User> GetByNormalizedEmailAsync(string normalizedEmail)
		{
			var query = Collection.Find(u => u.NormalizedEmail == normalizedEmail);
			return await query.FirstOrDefaultAsync();
		}

		public async Task<User> GetByUserNameAsync(string normalizedUserName)
		{
			var query = Collection.Find(u => u.NormalizedUserName == normalizedUserName);
			return await query.FirstOrDefaultAsync();
		}

		public IFindFluent<User, User> Search(UserSearchParamters paramters)
		{
			var filter = NotDeleted;

			if (!string.IsNullOrWhiteSpace(paramters.SearchValue))
			{
				if (Regex.Match(paramters.SearchValue, "^[0-9]+$").Success)
				{
					filter &= Builders<User>.Filter.Regex(u => u.PhoneNumber, $"/{paramters.SearchValue}/");
				}
				else
				{
					filter &= Builders<User>.Filter.Regex(u => u.Name, $"/{paramters.SearchValue}/i");
				}
			}

			if (!string.IsNullOrWhiteSpace(paramters.SortValue))
			{
				SortDefinition<User> sortDefinition;

				if (paramters.SortAscending)
				{
					sortDefinition = Builders<User>.Sort.Ascending(paramters.SortValue);
				}
				else
				{
					sortDefinition = Builders<User>.Sort.Descending(paramters.SortValue);
				}

				return Collection.Find(filter).Sort(sortDefinition);
			}

			return Collection.Find(filter);
		}
	}
}
