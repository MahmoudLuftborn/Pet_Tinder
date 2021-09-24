using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

using Pet_Tinder.Indentity.Extensions;
using Pet_Tinder.Repositories.Interfaces;

using System;
using System.Text;

namespace Pet_Tinder.Extensions
{
	public static class IdentityServiceCollectionExtention
	{
		public static IServiceCollection AddIdentityOptionsDI(this IServiceCollection services)
		{
			services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 0;
				// Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;
				// User settings.
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true;
			});
			return services;
		}

		public static IServiceCollection AddCookieConfiguration(this IServiceCollection services)
		{
			services.ConfigureApplicationCookie((options) =>
			{
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/Identity/Account/Login";
				options.AccessDeniedPath = "/Identity/Account/AccessDenied";
				options.ExpireTimeSpan = TimeSpan.FromDays(5);
				options.SlidingExpiration = true;

				options.Events.OnSigningIn = async (context) =>
				{
					var userRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
					var userId = context.Principal.GetUserId();
					var user = await userRepo.GetAsync(userId);

					if (user.ExtendedUserSession)
					{
						context.Properties.ExpiresUtc = DateTimeOffset.Now.AddDays(100);
						context.Properties.AllowRefresh = true;
					}

				};
			});


			return services;
		}

		public static IServiceCollection AddAuthenticationDI(this IServiceCollection services, IConfiguration Configuration)
		{
			var key = Encoding.ASCII.GetBytes(Configuration["JWT:Secret"]);
			var validAudience = Configuration["JWT:ValidAudience"];
			var validIssuer = Configuration["JWT:ValidIssuer"];

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidateAudience = true,
				RequireExpirationTime = true,
				ValidateLifetime = false,
				ValidAudience = validAudience,
				ValidIssuer = validIssuer,
			};

			services.AddSingleton(tokenValidationParameters);

			services.AddAuthentication()
				.AddJwtBearer(options =>
					{
						options.SaveToken = true;
						options.RequireHttpsMetadata = true;
						options.TokenValidationParameters = tokenValidationParameters.Clone();
						options.TokenValidationParameters.ValidateLifetime = true;
					});

			return services;
		}
	}
}
