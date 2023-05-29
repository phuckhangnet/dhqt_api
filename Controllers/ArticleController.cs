using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Article;
using Project.UseCases.ArticleMenu;

namespace ProjectBE.Controllers;
[Route("article")]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IMediator _mediator;
    public ArticleController(ILogger<ArticleController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddArticle([FromBody] AddArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    public async Task<IActionResult> GetListArticle([FromBody] GetArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("delete")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> DeleteArticle([FromBody] DeleteArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("menu")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> ArticleMenu([FromBody] GetArticleMenuCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
}
