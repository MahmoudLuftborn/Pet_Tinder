using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Pet_Tinder.Models.Domain;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Strauss.Frontend.Areas.Identity.Pages.Account.Manage
{
	public class ChangePassword : PageModel
	{
		private readonly ILogger<ChangePassword> _logger;
		private readonly IStringLocalizer _localizer;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public ChangePassword(
			ILogger<ChangePassword> logger,
			IStringLocalizer stringLocalizer,
			UserManager<User> userManager,
			SignInManager<User> signInManager)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_localizer = stringLocalizer ?? throw new ArgumentNullException(nameof(stringLocalizer));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
		}

		[BindProperty]
		public InputModel Input { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		public class InputModel
		{
			[Required]
			[DataType(DataType.Password)]
			public string OldPassword { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string NewPassword { get; set; }

			[DataType(DataType.Password)]
			[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var hasPassword = await _userManager.HasPasswordAsync(user);
			if (!hasPassword)
			{
				return RedirectToPage("./SetPassword");
			}

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
			if (!changePasswordResult.Succeeded)
			{
				foreach (var error in changePasswordResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return Page();
			}

			await _signInManager.RefreshSignInAsync(user);
			_logger.LogInformation("User changed their password successfully.");
			StatusMessage = _localizer["Update_Password_Msg"];

			return RedirectToPage();
		}
	}
}
