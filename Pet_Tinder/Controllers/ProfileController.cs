using Microsoft.AspNetCore.Mvc;
using Pet_Tinder.Models.Domain;
using Pet_Tinder.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pet_Tinder.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ProfileController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            var user = await _userRepository.GetByUserNameAsync(username.ToUpper());
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            var username = User.Identity.Name;
            var exisitingUser = await _userRepository.GetByUserNameAsync(username.ToUpper());
            await UpdateUserProfile(user, exisitingUser);
            return View(exisitingUser);
        }

        private async Task UpdateUserProfile(User user, User exisitingUser)
        {
            exisitingUser.Breed = user.Breed;
            exisitingUser.Type = user.Type;
            exisitingUser.HasBreedCertificate = user.HasBreedCertificate;
            exisitingUser.Gender = user.Gender;
            exisitingUser.Color = user.Color;
            exisitingUser.Description = user.Description;
            exisitingUser.Images = user.Images;

            await _userRepository.ReplaceAsync(exisitingUser);
        }
    }
}
