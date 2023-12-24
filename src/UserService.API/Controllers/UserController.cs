using UserService.API.Contract;
using UserService.API.Contract.Users;
using UserService.Application;
using UserService.Application.Models;
using UserService.Application.Ports;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
	/// <summary>
	/// Lists the existing users
	/// </summary>
	/// <response code="200">List of users</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponse>))]
	public async Task<IActionResult> Get(
		[FromServices] IAsyncRepository repository)
	{
		var users = await repository.GetAll<User>();

		return Ok(users.Select(u => UserResponse.From(u)));
	}

	/// <summary>
	/// Gets a user by ID
	/// </summary>
	/// <response code="200">The user</response>
	/// <response code="404">User not found</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
	public async Task<IActionResult> Get(
		[FromRoute] long id,
		[FromServices] IAsyncRepository repository)
	{
		var user = await repository.Get<User>(id);

		return user is null
			? NotFound(ErrorResponse.EntityNotFound())
			: Ok(UserResponse.From(user));
	}

	/// <summary>
	/// Creates a user
	/// </summary>
	/// <response code="200">ID of the created user</response>
	/// <response code="400">Validation error</response>
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
	[HttpPost]
	public async Task<IActionResult> Post(
		[FromBody] UserCreationRequest model,
		[FromServices] UserCreator userCreator)
	{        
		var result = await userCreator.Create(model);

		return result.IsFailure
			? BadRequest(ErrorResponse.From(result.Error))
			: Ok(result.Value);
	}

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <response code="200">User successfully updated</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">User not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Put(
        [FromRoute] long id,
        [FromBody] UserUpdateRequest model,
        [FromServices] IAsyncRepository repository,
        [FromServices] UserUpdater userUpdater)
    {
        var updateResult = await userUpdater.Update(id, model);

        return updateResult.IsFailure
            ? BadRequest(ErrorResponse.From(updateResult.Error))
            : Ok(updateResult.Value);
    }
}