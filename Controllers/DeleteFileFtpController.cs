using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.File;

namespace ProjectBE.Controllers;
[Route("deleteftp")]
public class FileController : Controller
{
    private readonly ILogger<FileController> _logger;
    private readonly IMediator _mediator;
    public FileController(ILogger<FileController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("delete_wh")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    public async Task<IActionResult> DeleteFile([FromBody] DeleteFileCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }

}
