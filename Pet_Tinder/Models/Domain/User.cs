using Microsoft.AspNetCore.Identity;

using Pet_Tinder.Pet_Tinder.Models.Domain.Interfaces;
using Pet_Tinder.Models.DTO;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using Pet_Tinder.Models.Domain.Enums;

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


        #region Pet data
        public string OwnerName { get; set; }
        public Breed Breed { get; set; }
        public PetType Type { get; set; }
        public Gender Gender { get; set; }
        public bool HasBreedCertificate { get; set; }
        public Color Color { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        #endregion

        public User()
        {
            Roles = new List<string>();
            LoginInfo = new List<UserLoginInfo>();
            Claims = new List<Claim>();
            Images = new List<string>();
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
