using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Strauss.Frontend.Areas.Identity.IdentityHostingStartup))]
namespace Strauss.Frontend.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
			});
		}
	}
}
