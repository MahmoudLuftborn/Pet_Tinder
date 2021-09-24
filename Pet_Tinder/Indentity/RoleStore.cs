using Microsoft.AspNetCore.Identity;

using System.Threading;
using System.Threading.Tasks;

namespace Pet_Tinder.Indentity
{
	public class RoleStore : IRoleStore<string>
	{
		public Task<IdentityResult> CreateAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(IdentityResult.Success);
		}

		public Task<IdentityResult> DeleteAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(IdentityResult.Success);
		}


		public Task<string> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			return Task.FromResult(roleId);
		}

		public Task<string> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			return Task.FromResult(normalizedRoleName);
		}

		public Task<string> GetNormalizedRoleNameAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role);
		}

		public Task<string> GetRoleIdAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role);
		}

		public Task<string> GetRoleNameAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(role);
		}

		public Task SetNormalizedRoleNameAsync(string role, string normalizedName, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;

		}

		public Task SetRoleNameAsync(string role, string roleName, CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public Task<IdentityResult> UpdateAsync(string role, CancellationToken cancellationToken)
		{
			return Task.FromResult(IdentityResult.Success);
		}

		public void Dispose()
		{

		}
	}
}