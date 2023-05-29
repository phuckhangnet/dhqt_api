using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Role;

namespace ProjectBE.Controllers;
[Route("role")]
public class RoleController : Controller
{
    private readonly ILogger<RoleController> _logger;
    private readonly IMediator _mediator;
    public RoleController(ILogger<RoleController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddRole([FromBody] AddRoleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    [AuthorizeAttribute]
    public async Task<IActionResult> GetListRole([FromBody] GetRoleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
