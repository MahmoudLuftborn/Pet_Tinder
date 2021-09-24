using MongoDB.Driver;

using Pet_Tinder.Models.Domain;
using Pet_Tinder.Models.DTO.Search;

using System.Threading.Tasks;

namespace Pet_Tinder.Repositories.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User> GetByUserNameAsync(string UserName);
		Task<User> GetByEmailAsync(string normalizedEmail);
		Task<User> GetByNormalizedEmailAsync(string normalizedEmail);
		IFindFluent<User, User> Search(UserSearchParamters paramters);
	}
}
