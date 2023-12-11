using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.UI;

namespace ProjectBE.Controllers;
[Route("UI")]
public class UIController : Controller
{
    private readonly ILogger<UIController> _logger;
    private readonly IMediator _mediator;
    public UIController(ILogger<UIController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("query")]
    public async Task<IActionResult> GetListUI([FromBody] GetUICommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
