using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.WarehouseFile;

namespace ProjectBE.Controllers;
[Route("warehousefile")]
public class WarehouseFileController : Controller
{
    private readonly ILogger<WarehouseFileController> _logger;
    private readonly IMediator _mediator;
    public WarehouseFileController(ILogger<WarehouseFileController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("query")]
    public async Task<IActionResult> GetListWarehouseFile([FromBody] GetWarehouseFileCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
