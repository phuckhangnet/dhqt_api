using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Banner;

namespace ProjectBE.Controllers;
[Route("Banner")]
public class BannerController : Controller
{
    private readonly ILogger<BannerController> _logger;
    private readonly IMediator _mediator;
    public BannerController(ILogger<BannerController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("query")]
    public async Task<IActionResult> GetListBanner([FromBody] GetBannerCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
