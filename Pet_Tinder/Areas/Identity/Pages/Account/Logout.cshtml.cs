using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Pet_Tinder.Models.Domain;

using System;
using System.Threading.Tasks;

namespace Strauss.Frontend.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class Logout : PageModel
	{
		private readonly ILogger<Logout> _logger;
		private readonly SignInManager<User> _signInManager;

		public Logout(ILogger<Logout> logger, SignInManager<User> signInManager)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");

			return LocalRedirect(returnUrl);
		}
	}
}
