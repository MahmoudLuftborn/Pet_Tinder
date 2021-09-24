using System;

namespace Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces
{
	public interface IEntity
	{
		string Id { get; set; }
		DateTime CreatedAt { get; set; }
		DateTime? ModifiedAt { get; set; }
		DateTime? DeletedAt { get; set; }
	}
}