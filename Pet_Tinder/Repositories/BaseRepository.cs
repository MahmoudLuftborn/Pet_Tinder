using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using Pet_Tinder.Repositories.Interfaces;
using Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pet_Tinder.Repositories
{
	public class BaseRepository<Entity> : IRepository<Entity> where Entity : IEntity
	{
		protected readonly ILogger<BaseRepository<Entity>> _logger;
		protected readonly ISystemClock _systemClock;

		protected IMongoCollection<Entity> Collection { get; }
		protected FilterDefinition<Entity> NotDeleted
		{
			get
			{
				return Builders<Entity>.Filter.Eq(e => e.DeletedAt, null);
			}
		}

		public BaseRepository(ILogger<BaseRepository<Entity>> logger,
								IMongoDatabase database,
								ISystemClock systemClock)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));

			Collection = database.GetCollection<Entity>(typeof(Entity).Name);
		}


		#region Utilities

		public long Count(Expression<Func<Entity, bool>> filter)
		{
			return Collection.CountDocuments(filter);
		}

		public async Task<long> CountAsync(Expression<Func<Entity, bool>> filter)
		{
			return await Collection.CountDocumentsAsync(filter);
		}

		#endregion

		#region Add

		public virtual void Add(Entity entity)
		{
			entity.CreatedAt = _systemClock.UtcNow.DateTime;
			Collection.InsertOne(entity);
		}

		public virtual async Task AddAsync(Entity entity)
		{
			entity.CreatedAt = _systemClock.UtcNow.DateTime;
			await Collection.InsertOneAsync(entity);
		}

		public virtual async Task AddAsync(ICollection<Entity> entities)
		{
			(entities as List<Entity>).ForEach(e => e.CreatedAt = _systemClock.UtcNow.DateTime);
			await Collection.InsertManyAsync(entities);
		}
		#endregion

		#region Delete

		public void Delete(Entity entity)
		{
			Update(entity.Id, new Dictionary<string, object>() { { nameof(entity.DeletedAt), _systemClock.UtcNow.DateTime } });
		}

		public void Delete(string id)
		{
			Update(id, new Dictionary<string, object>() { { "DeletedAt", _systemClock.UtcNow.DateTime } });
		}

		public virtual async Task DeleteAsync(Entity entity)
		{
			await UpdateAsync(entity.Id, new Dictionary<string, object>() { { nameof(entity.DeletedAt), _systemClock.UtcNow.DateTime } });
		}

		public async Task DeleteAsync(string id)
		{
			await UpdateAsync(id, new Dictionary<string, object>() { { "DeletedAt", _systemClock.UtcNow.DateTime } });
		}

		#endregion

		#region Update

		public virtual void Replace(Entity entity)
		{
			entity.ModifiedAt = _systemClock.UtcNow.DateTime;
			Collection.ReplaceOne(x => x.Id == entity.Id, entity);
		}

		public virtual async Task ReplaceAsync(Entity entity)
		{
			entity.ModifiedAt = _systemClock.UtcNow.DateTime;
			await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

		}

		public virtual void Update(string id, Dictionary<string, object> updatedFields)
		{
			var filter = Builders<Entity>.Filter.Eq(x => x.Id, id);

			var update = Builders<Entity>.Update.CurrentDate(x => x.ModifiedAt);

			update = updatedFields.Aggregate(update, (current, item) => current.Set(item.Key, item.Value));

			Collection.UpdateOne(filter, update, new UpdateOptions() { IsUpsert = true });
		}

		public virtual async Task UpdateAsync(string id, Dictionary<string, object> updatedFields, CancellationToken cancellationToken = default)
		{
			var filter = Builders<Entity>.Filter.Eq(x => x.Id, id);

			var update = Builders<Entity>.Update.CurrentDate(x => x.ModifiedAt);

			update = updatedFields.Aggregate(update, (current, item) => current.Set(item.Key, item.Value));

			await Collection.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true }, cancellationToken: cancellationToken);
		}

		#endregion

		#region Get

		public virtual ICollection<Entity> Get()
		{
			return Collection.Find(NotDeleted).ToList();
		}

		public virtual async Task<ICollection<Entity>> GetAsync()
		{
			return await Collection.Find(NotDeleted).ToListAsync();
		}

		public ICollection<Entity> Get(Expression<Func<Entity, bool>> expression)
		{
			return Collection.Find(expression).ToList();
		}

		public async Task<ICollection<Entity>> GetAsync(Expression<Func<Entity, bool>> expression)
		{
			return await Collection.Find(expression).ToListAsync();
		}

		public virtual Entity Get(string id)
		{
			var filter = Builders<Entity>.Filter.And(NotDeleted, Builders<Entity>.Filter.Eq(e => e.Id, id));
			var entity = Collection.Find(filter).FirstOrDefault();
			return entity;
		}

		public virtual async Task<Entity> GetAsync(string id)
		{
			var filter = Builders<Entity>.Filter.And(NotDeleted, Builders<Entity>.Filter.Eq(e => e.Id, id));
			var entity = await Collection.Find(filter).FirstOrDefaultAsync();
			return entity;
		}
		#endregion
	}
}
