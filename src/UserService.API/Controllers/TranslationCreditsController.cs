using Microsoft.AspNetCore.Mvc;
using UserService.API.Contract;
using UserService.API.Contract.Users;
using UserService.Application;
using UserService.Domain;

namespace UserService.API.Controllers;

[ApiController]
[Route("users/{userId}/credits")]
public class TranslationCreditsController : ControllerBase
{
    private readonly UserCreditsService _userCreditsService;

    public TranslationCreditsController(UserCreditsService userCreditsService)
    {
        _userCreditsService = userCreditsService;
    }

    /// <summary>
    /// Adds credits to a user's account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="request">The credit modification request.</param>
    /// <response code="200">Credits successfully added.</response>
    /// <response code="400">Validation error or unable to add credits.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddCredits(long userId, [FromBody] TranslationCreditsRequest request)
    {
        var result = await _userCreditsService.AddCredits(userId, request.Credits);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error.Message == "User not found")
        {
            return NotFound(ErrorResponse.EntityNotFound());
        }

        return BadRequest(ErrorResponse.From(result.Error));
    }

    /// <summary>
    /// Spends credits on behalf of a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="request">The credit modification request.</param>
    /// <response code="200">Credits successfully spent.</response>
    /// <response code="400">Validation error or insufficient credits.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("spend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SpendCredits(long userId, [FromBody] TranslationCreditsRequest request)
    {
        var result = await _userCreditsService.SpendCredits(userId, request.Credits);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error.Message == "User not found")
        {
            return NotFound(ErrorResponse.EntityNotFound());
        }

        return BadRequest(ErrorResponse.From(result.Error));
    }

    /// <summary>
    /// Subtracts credits from a user's account.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="request">The credit modification request.</param>
    /// <response code="200">Credits successfully subtracted.</response>
    /// <response code="400">Validation error or unable to subtract credits.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("subtract")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubtractCredits(long userId, [FromBody] TranslationCreditsRequest request)
    {
        var result = await _userCreditsService.SubtractCredits(userId, request.Credits);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error.Message == "User not found")
        {
            return NotFound(ErrorResponse.EntityNotFound());
        }

        return BadRequest(ErrorResponse.From(result.Error));
    }
}