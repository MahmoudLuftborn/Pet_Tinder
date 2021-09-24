using Microsoft.AspNetCore.Identity;

using Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces;
using Pet_Tinder.Models.DTO;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Pet_Tinder.Models.Domain
{
	public class User : IdentityUser, IEntity
	{
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public List<string> Roles { get; set; }
		public string SignalRToken { get; set; }
		public bool ExtendedUserSession { get; set; }

		public List<UserLoginInfo> LoginInfo { get; set; }
		public List<Claim> Claims { get; set; }

		public User()
		{
			Roles = new List<string>();
			LoginInfo = new List<UserLoginInfo>();
			Claims = new List<Claim>();
		}

		public User(UserDTO userDTO) : this()
		{
			Id = userDTO.Id ?? Guid.NewGuid().ToString();
			Name = userDTO.Name;
			Roles = userDTO.Roles;
			PhoneNumber = userDTO.PhoneNumber;
			Email = userDTO.Email;
		}
	}
}
