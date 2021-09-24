using Pet_Tinder.Models.Domain.Enums;
using System;

namespace Pet_Tinder.Models.DTO.Search
{
	public class UserSearchParamters// : BaseSearchParamters
	{
		public Region? Region { get; set; }
		public PetType PetType { get; set; }
        public Gender Gender { get; set; }
        public Breed? Breed { get; set; }
        public Color? Color { get; set; }
        public bool? HasBreedCertificate { get; set; }
    }
}