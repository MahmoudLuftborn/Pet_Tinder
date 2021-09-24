using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

using Pet_Tinder.Models.Domain;
using Pet_Tinder.Repositories.Interfaces;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Strauss.Frontend.Areas.Identity.Pages.Account.Manage
{
	public partial class IndexModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly IUserRepository _userRepository;
		private readonly SignInManager<User> _signInManager;
		private readonly IStringLocalizer _localizer;

		public IndexModel(
			UserManager<User> userManager,
			IUserRepository userRepository,
			SignInManager<User> signInManager,
			IStringLocalizer stringLocalizer)
		{
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
			_localizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
		}

		public string Username { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Phone]
			[DataType(DataType.PhoneNumber)]
			public string PhoneNumber { get; set; }
			public string Name { get; set; }
			public string Role { get; set; }

			[EmailAddress]
			public string Email { get; set; }
			public string Store { get; set; }
		}

		private void Load(User user)
		{
			Username = user.UserName;

			Input = new InputModel
			{
				PhoneNumber = user.PhoneNumber,
				Email = user.Email,
				Name = user.Name,
				Role = user.Roles[0],
			};
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			Load(user);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (!ModelState.IsValid)
			{
				Load(user);
				return Page();
			}

			_userRepository.Update(user.Id, new Dictionary<string, object>
				{
					{ nameof(user.Name), Input.Name},
					{ nameof(user.PhoneNumber), Input.PhoneNumber},
					{ nameof(user.Email), Input.Email},
				});

			await _signInManager.RefreshSignInAsync(user);
			StatusMessage = _localizer["Update_Profile_Msg"];
			return RedirectToPage();
		}
	}
}
