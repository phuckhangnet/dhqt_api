using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Blog;

namespace ProjectBE.Controllers;
[Route("Blog")]
public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;
    private readonly IMediator _mediator;
    public BlogController(ILogger<BlogController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddBlog([FromBody] AddBlogCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    public async Task<IActionResult> GetListBlog([FromBody] GetBlogCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateBlog([FromBody] UpdateBlogCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("likes")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> LikesBlog([FromBody] LikesBlogCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpDelete("delete")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> DeleteBlog([FromBody] DeleteBlogCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
