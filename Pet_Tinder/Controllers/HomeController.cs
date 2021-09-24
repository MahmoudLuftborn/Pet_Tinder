using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Pet_Tinder.Attributes;
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

        public IActionResult Index( )
        {
            return View();
        }

        public async Task<IActionResult> Search(UserSearchParamters userSearchParamters)
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
            var breeds = new List<string>();

            foreach (var breed in allBreeds)
            {
                var breedPetType =
                  typeof(Breed)
                     .GetMember(breed.ToString())
                     .FirstOrDefault()
                     .GetCustomAttributes(typeof(PetTypeAttribute), false)
                     .Cast<PetTypeAttribute>()
                     .SingleOrDefault().Type;

                if (breedPetType == type)
                {
                    breeds.Add(breed.ToString());
                }
            }

            return Ok(breeds);
        }
    }
}
