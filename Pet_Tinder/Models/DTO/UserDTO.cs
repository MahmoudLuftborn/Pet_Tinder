using Pet_Tinder.Models.Domain;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pet_Tinder.Models.DTO
{
	public class UserDTO
	{
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public List<string> Roles { get; set; }

		[Required]
		[Phone]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		public UserDTO()
		{
			Roles = new List<string>();
		}

		public UserDTO(User user)
		{
			Id = user.Id;
			Name = user.Name ?? user.UserName;
			PhoneNumber = user.PhoneNumber;
			Email = user.Email;
			Roles = user.Roles;
		}
	}
}
