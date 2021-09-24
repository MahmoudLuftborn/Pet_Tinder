using Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pet_Tinder.Repositories.Interfaces
{
	public interface IRepository<T> where T : IEntity
	{
		void Add(T entity);
		Task AddAsync(T entity);
		Task AddAsync(ICollection<T> entities);
		void Replace(T entity);
		Task ReplaceAsync(T entity);
		void Update(string id, Dictionary<string, object> updatedFields);
		Task UpdateAsync(string id, Dictionary<string, object> updatedFields, CancellationToken cancellationToken = default);
		void Delete(T entity);
		Task DeleteAsync(T entity);
		void Delete(string id);
		Task DeleteAsync(string id);
		ICollection<T> Get();
		Task<ICollection<T>> GetAsync();
		T Get(string id);
		Task<T> GetAsync(string id);
		ICollection<T> Get(Expression<Func<T, bool>> expression);
		Task<ICollection<T>> GetAsync(Expression<Func<T, bool>> expression);
		long Count(Expression<Func<T, bool>> filter);
		Task<long> CountAsync(Expression<Func<T, bool>> filter);
	}
}
