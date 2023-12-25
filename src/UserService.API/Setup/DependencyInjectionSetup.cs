using UserService.Application;
using UserService.Application.Ports;
using UserService.Infrastructure.EntityFramework;

namespace UserService.API.Setup;

public static class DependencyInjectionSetup
{
	public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped(typeof(IAsyncRepository), typeof(EfAsyncRepository));
		services.AddScoped<UserCreator>();
		services.AddScoped<UserUpdater>();
		services.AddScoped<UserCreditsService>();
		//services.AddScoped<UserSynchronizer>();

		//services.AddHttpClient<IUserProvider, SomeUserProvider>((svc, httpClient) =>
		//{
		//	var host = configuration.GetSection("SomeUserProvider")["Host"];
		//	httpClient.BaseAddress = new Uri(host!);
		//});
	}
}
