using Microsoft.AspNetCore.Identity;

using Pet_Tinder.Models.Domain;
using Pet_Tinder.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Pet_Tinder.Indentity
{
	public class UserStore :
			IUserStore<User>, IUserPasswordStore<User>,
			IUserEmailStore<User>,
			IUserPhoneNumberStore<User>,
			IUserRoleStore<User>,
			IUserLoginStore<User>,
			IUserClaimStore<User>
	{
		private readonly IUserRepository _userRepository;

		public UserStore(IUserRepository userRepository)
		{
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
		}

		#region IUserStore
		public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
		{
			await _userRepository.ReplaceAsync(user);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
		{
			await _userRepository.DeleteAsync(user);
			return IdentityResult.Success;
		}

		public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			return _userRepository.GetAsync(userId);
		}

		public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			return _userRepository.GetByUserNameAsync(normalizedUserName);
		}

		public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.NormalizedUserName);
		}

		public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Id);
		}

		public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.UserName);
		}

		public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
		{
			user.NormalizedUserName = normalizedName;
			var updatedFields = new Dictionary<string, object> { { nameof(user.NormalizedUserName), normalizedName } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
		{
			user.UserName = userName;
			var updatedFields = new Dictionary<string, object> { { nameof(user.UserName), userName } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
		{
			await _userRepository.ReplaceAsync(user);
			return IdentityResult.Success;
		}

		#endregion

		#region IUserPasswordStore
		public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PasswordHash);
		}


		public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
		{
			user = await _userRepository.GetAsync(user.Id);
			return !string.IsNullOrEmpty(user.PasswordHash);
		}


		public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
		{
			user.PasswordHash = passwordHash;
			var updatedFields = new Dictionary<string, object> { { nameof(user.PasswordHash), passwordHash } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		#endregion

		#region IUserEmailStore

		public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			return await _userRepository.GetByNormalizedEmailAsync(normalizedEmail);
		}

		public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.EmailConfirmed);
		}

		public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.NormalizedEmail);
		}

		public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
		{
			user.Email = email;
			var updatedFields = new Dictionary<string, object> { { nameof(user.Email), email } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
		{
			user.EmailConfirmed = confirmed;
			var updatedFields = new Dictionary<string, object> { { nameof(user.EmailConfirmed), confirmed } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
		{
			user.NormalizedEmail = normalizedEmail;
			var updatedFields = new Dictionary<string, object> { { nameof(user.NormalizedEmail), normalizedEmail } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		#endregion

		#region IUserPhoneNumberStore
		public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PhoneNumber);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.PhoneNumberConfirmed);
		}

		public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
		{
			user.PhoneNumber = phoneNumber;
			var updatedFields = new Dictionary<string, object> { { nameof(user.PhoneNumber), phoneNumber } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
		{
			user.PhoneNumberConfirmed = confirmed;
			var updatedFields = new Dictionary<string, object> { { nameof(user.PhoneNumberConfirmed), confirmed } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		#endregion

		#region IUserRoleStore<TUser>
		public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			user.Roles.Add(roleName);
			user.Roles = user.Roles.Distinct().ToList();
			var updatedFields = new Dictionary<string, object> { { nameof(user.Roles), user.Roles } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			user.Roles.Remove(roleName);
			user.Roles = user.Roles.Distinct().ToList();
			var updatedFields = new Dictionary<string, object> { { nameof(user.Roles), user.Roles } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
		{
			user = await _userRepository.GetAsync(user.Id);
			return user.Roles;
		}

		public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
		{
			user = await _userRepository.GetAsync(user.Id);
			return user.Roles.Any(r => r.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));
		}

		public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAsync(user => user.Roles.Any(r => r == roleName));
			return users.ToList();
		}
		#endregion

		#region IUserLoginStore
		public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
		{
			user.LoginInfo.Add(login);
			var updatedFields = new Dictionary<string, object> { { nameof(user.LoginInfo), user.LoginInfo } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			var loginInfo = user.LoginInfo.FirstOrDefault(info => info.LoginProvider == loginProvider && info.ProviderKey == providerKey);
			user.LoginInfo.Remove(loginInfo);

			var updatedFields = new Dictionary<string, object> { { nameof(user.LoginInfo), user.LoginInfo } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
		{
			user = await _userRepository.GetAsync(user.Id);
			return user.LoginInfo;
		}

		public async Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAsync(user => user.LoginInfo.Any(info => info.LoginProvider == loginProvider && info.ProviderKey == providerKey));
			return users.FirstOrDefault();
		}
		#endregion

		#region IUserClaimStore
		public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
		{
			return Task.FromResult(user.Claims as IList<Claim>);
		}

		public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			user.Claims.AddRange(claims);
			var updatedFields = new Dictionary<string, object> { { nameof(user.Claims), user.Claims } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
		{
			user.Claims.Remove(claim);
			user.Claims.Add(newClaim);
			var updatedFields = new Dictionary<string, object> { { nameof(user.Claims), user.Claims } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
		{
			foreach (var claim in claims)
			{
				user.Claims.Remove(claim);
			}

			var updatedFields = new Dictionary<string, object> { { nameof(user.Claims), user.Claims } };
			await _userRepository.UpdateAsync(user.Id, updatedFields, cancellationToken);
		}

		public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAsync(user => user.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value && c.Issuer == claim.Issuer));
			return users.ToList();
		}
		#endregion

		public void Dispose()
		{

		}
	}
}