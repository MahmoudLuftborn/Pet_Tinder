using Pet_Tinder.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pet_Tinder.Models.Domain.Enums
{
    public enum Breed
    {
        [PetType(PetType.Cat)]
        Munchkin,
        [PetType(PetType.Cat)]
        Ocicat,
        [PetType(PetType.Cat)]
        Oriental,
        [PetType(PetType.Cat)]
        Persian,
        [PetType(PetType.Cat)]
        Peterbald,
        [PetType(PetType.Cat)]
        Pixiebob,

        [PetType(PetType.Dog)]
        Afghan,
        [PetType(PetType.Dog)]
        Alaskan,
        [PetType(PetType.Dog)]
        Husky,
        [PetType(PetType.Dog)]
        Boston,
        [PetType(PetType.Dog)]
        Bugg
    }
}
