using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Pet_Tinder.Models.Domain;
using Pet_Tinder.Repositories.Interfaces;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Strauss.Frontend.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class Login : PageModel
	{
		private readonly ILogger<Login> _logger;
		private readonly IUserRepository _userRepository;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public Login(
			ILogger<Login> logger,
			IUserRepository userRepository,
			SignInManager<User> signInManager,
			UserManager<User> userManager)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[Required]
			public string UserName { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			[Display(Name = "Remember me?")]
			public bool RememberMe { get; set; }
		}

		public async Task<IActionResult> OnGetAsync(string returnUrl = "/")
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;

			if (User.Identity.IsAuthenticated)
			{
				return LocalRedirect(returnUrl);
			}
			else
			{
				return Page();
			}
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
		{
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (!ModelState.IsValid)
			{
				return Page();
			}

			User user;
			if (Regex.Match(Input.UserName, "[0-9]+").Success)
			{
				var match = await _userRepository.GetAsync(u => u.DeletedAt == null && u.PhoneNumber == Input.UserName);
				user = match.SingleOrDefault();
				Input.UserName = user?.UserName;
			}
			else
			{
				user = await _userManager.FindByNameAsync(Input.UserName);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true

			var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				_logger.LogInformation($"User {Input.UserName} logged in.");

				return LocalRedirect(returnUrl);
			}
			if (result.RequiresTwoFactor)
			{
				return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
			}
			if (result.IsLockedOut)
			{
				_logger.LogWarning("User account locked out.");

				return RedirectToPage("./Lockout");
			}

			ModelState.AddModelError(string.Empty, "login error message");

			return Page();
		}
	}
}
