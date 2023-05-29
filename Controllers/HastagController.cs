using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Hastag;

namespace ProjectBE.Controllers;
[Route("hastag")]
public class HastagController : Controller
{
    private readonly ILogger<HastagController> _logger;
    private readonly IMediator _mediator;
    public HastagController(ILogger<HastagController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddHastag([FromBody] AddHastagCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    [AuthorizeAttribute]
    public async Task<IActionResult> GetListHastag([FromBody] GetHastagCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateHastag([FromBody] UpdateHastagCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
