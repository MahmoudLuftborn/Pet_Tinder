using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces;

using System;

namespace Pet_Tinder.Models.Domain
{
	public class BaseEntity : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
