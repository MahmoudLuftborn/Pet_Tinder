using Pet_Tinder.Models.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet_Tinder.Attributes
{
    public class PetTypeAttribute : Attribute
    {
        public PetType Type { get; set; }

        public PetTypeAttribute(PetType type)
        {
            Type = type;
        }
    }
}
