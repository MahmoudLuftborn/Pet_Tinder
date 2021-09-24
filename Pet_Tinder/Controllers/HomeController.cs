using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Pet_Tinder.Models;
using Pet_Tinder.Models.Domain.Enums;
using Pet_Tinder.Models.DTO.Search;
using Pet_Tinder.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pet_Tinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IActionResult> Index(UserSearchParamters userSearchParamters)
        {
            var username = User.Identity.Name;
            var user = await _userRepository.GetByUserNameAsync(username.ToUpper());
            userSearchParamters.PetType = user.Type;
            userSearchParamters.Gender = user.Gender;
            var pets = await _userRepository.Search(userSearchParamters).ToListAsync();

            return View(pets);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Breed(PetType type)
        {
            var allBreeds = Enum.GetValues(typeof(Breed));
            var breeds = new List<Breed>();

            foreach (var breed in allBreeds)
            {
                var sdfdf = typeof(PetType)
                     .GetMembers();

                var breedPetType =
                  typeof(Breed)
                     .GetMember(breed.ToString())
                     .Where(member => member.MemberType == MemberTypes.Field)
                     .FirstOrDefault()
                     .GetCustomAttributes(typeof(PetType), false)
                     .Cast<PetType>()
                     .SingleOrDefault();

                if (breedPetType == type)
                {
                    breeds.Add((Breed)breed);
                }
            }

            return Ok(breeds);
        }
    }
}
