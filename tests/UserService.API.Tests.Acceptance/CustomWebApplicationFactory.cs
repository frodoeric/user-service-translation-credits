using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using RichardSzalay.MockHttp;
using System.Net.Http;

namespace UserService.API.Test.Acceptance;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	public readonly MockHttpMessageHandler MockHttpMessageHandler = new();

	public CustomWebApplicationFactory() { }

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			services.AddTransient(typeof(HttpMessageHandlerBuilder),
				svc => new CustomHttpMessageHandlerBuilder(MockHttpMessageHandler));
		});

		builder.ConfigureAppConfiguration((context, configBuilder) =>
		{
			configBuilder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.Tests.json"));
		});
	}

	/// <summary>
	/// Replaces the application's HttpMessageHandlerBuilder so that our MockHttpMessageHandler
	/// is used for all external requests done with an HttpClient.
	/// </summary>
	public class CustomHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
	{
		public override string Name { get; set; } = string.Empty;
		public override HttpMessageHandler PrimaryHandler { get; set; }
		public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

		public CustomHttpMessageHandlerBuilder(HttpMessageHandler httpMessageHandler) =>
			PrimaryHandler = httpMessageHandler;

		public override HttpMessageHandler Build() => PrimaryHandler;
	}
}
