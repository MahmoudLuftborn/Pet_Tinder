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
            var filter = Builders<User>.Filter.Eq(x => x.Type, paramters.PetType);
            filter &= Builders<User>.Filter.Eq(x => x.Gender, paramters.Gender);

            if (paramters.Breed.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.Breed, paramters.Breed.Value);
            }

            if (paramters.Color.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.Color, paramters.Color.Value);
            }

            if (paramters.Region.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.Region, paramters.Region.Value);
            }

            if (paramters.HasBreedCertificate.HasValue)
            {
                filter &= Builders<User>.Filter.Eq(x => x.HasBreedCertificate, paramters.HasBreedCertificate.Value);
            }

            return Collection.Find(filter);
        }
    }
}
