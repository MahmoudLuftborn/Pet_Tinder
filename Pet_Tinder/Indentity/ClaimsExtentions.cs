using System.Security.Claims;

namespace Pet_Tinder.Indentity.Extensions
{
	public static class ClaimsExtentions
	{
		public static string GetUserId(this ClaimsPrincipal User)
		{
			var claim = User.FindFirst(ClaimTypes.NameIdentifier);
			return claim?.Value;
		}
	}
}
