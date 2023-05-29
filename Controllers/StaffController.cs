using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Users;

namespace ProjectBE.Controllers;
[Route("user")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator _mediator;
    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddUser([FromBody] AddUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    [AuthorizeAttribute]
    public async Task<IActionResult> GetListUser([FromBody] GetUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update-status")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("login")]
    //[DecryptedAttribute]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("change_password")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> ChangePasswordUser([FromBody] ChangePasswordUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("reset_password")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> ResetPasswordUser([FromBody] ResetPasswordUserCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
