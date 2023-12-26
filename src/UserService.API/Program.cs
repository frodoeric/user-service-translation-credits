using UserService.API.ErrorResponseHandling;
using UserService.API.Setup;
using UserService.Infrastructure.Services;
using UserService.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using UserService.API.Contract.Users.Validator;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
	.AddUserSecrets<Program>()
	.AddEnvironmentVariables();

builder.Services.AddControllers(options =>
	{
#if !DEBUG
		options.Filters.Add<ExceptionHandlingFilter>();
#endif
	})
	.ConfigureApiBehaviorOptions(options =>
	{
		options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.Create;
	});

builder.Services.AddFluentValidationAutoValidation().
	AddValidatorsFromAssemblyContaining<UserCreationRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
//if (EnableBackgroundJobs)
//	builder.Services.AddBackgroundJobs(builder.Configuration);

builder.Services.AddScoped<ICrmService, CrmService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.EnsurePersistenceIsReady();

//if (EnableBackgroundJobs)
//	app.ConfigureBackgroundJobs();

Configuration = app.Configuration;

app.Run();

public partial class Program
{
	public static IConfiguration? Configuration { get; private set; }
	public static bool EnableBackgroundJobs = true;
}