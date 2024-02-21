using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Menu;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ProjectBE.Controllers;
[Route("menu")]
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public MenuController(ILogger<MenuController> logger, IMediator mediator, IConfiguration config)
    {
        _logger = logger;
        _mediator = mediator;
        _config = config;
    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddMenu([FromBody] AddMenuCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("query")]
    [AuthorizeAttribute]
    public async Task<IActionResult> GetListMenu([FromBody] GetMenuCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("update")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateMenu([FromBody] UpdateMenuCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("get-webpage")]
    //[AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> GetPageByMenu([FromBody] GetPageByMenuCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }

    [HttpGet("getall")]
    [AuthorizeAttribute]
    public async Task<IEnumerable<dynamic>> getall()
    {

        using (IDbConnection conn = new SqlConnection(_config.GetConnectionString("IU_BT_DB")))
        {
            string sQuery = ""
            + " SELECT *, CASE WHEN ISPAGE = 1 AND ISACTIVE = 1 THEN 'PAGE' WHEN ISPAGE = 0 AND ISACTIVE = 1 THEN 'ARTICLE' ELSE '' END TYPEMENU  "
            + " FROM Menu ";
            conn.Open();
            var result = await conn.QueryAsync<dynamic>(sQuery);
            return result;
        }

    }

}
