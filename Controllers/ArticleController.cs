using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Article;
using Project.UseCases.ArticleMenu;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.Data.SqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Dapper;
using InfluxDB3.Client;
using InfluxDB3.Client.Write;
using System.Diagnostics;
using ProjectBE.Models;

namespace ProjectBE.Controllers;
[Route("article")]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public ArticleController(ILogger<ArticleController> logger, IMediator mediator, IConfiguration config)
    {
        _logger = logger;
        _mediator = mediator;
        _config = config;

    }
    [HttpPost("add")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddArticle([FromBody] AddArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("add_draft")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> AddDraftArticle([FromBody] AddDraftArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("post_draft")]
    //[DecryptedAttribute] // Chỉ sử dụng attribute này cho những route cần Encrypt request!!
    [AuthorizeAttribute]
    public async Task<IActionResult> PostDraftArticle([FromBody] PostDraftArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
   

    [HttpPost("query")]
    public async Task<IActionResult> GetListArticle([FromBody] GetArticleCommand command, [FromServices] IMediator mediator)
    {
        
        var time = DateTime.UtcNow;
        var result = await mediator.Send(command);

        try {

            if (_config.GetValue<bool>("Logging:Allow"))
            {
                var hostUrl = InfluxDBModel.hostUrl;

                using var client = new InfluxDBClient(hostUrl, token: InfluxDBModel.token);

                const string database = "hcmiu";

                var points = new PointData[]
                    {
                        PointData.Measurement("Store")
                        .SetField("Query", (DateTime.UtcNow - time).TotalSeconds),


                    };

                await client.WritePointsAsync(points: points, database: database);
            }
           




            

        }
        catch { }
        

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
    [HttpPost("update_no_menu")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UpdateNoMenuArticle([FromBody] UpdateNoMenuArticleCommand command, [FromServices] IMediator mediator)
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
    [HttpPost("link")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> LinkArticle([FromBody] LinkArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }
    [HttpPost("unlink")]
    [AuthorizeAttribute]
    //[DecryptedAttribute]
    public async Task<IActionResult> UnlinkArticle([FromBody] UnlinkArticleCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return StatusCode((int)result.STATUSCODE, result);
    }

    [HttpPost("GetArticleContent")]
    [AuthorizeAttribute]
    public async Task<IActionResult> GetArticleContent([FromBody] GetArticleContentCommand command, [FromServices] IMediator mediator)
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
            + " SELECT a.ID, a.TITLE, a.SUMMARY, a.HASTAG, a.AVATAR, a.PRIORITYLEVEL, a.LANGUAGE, a.CREATEDATE, a.LATESTEDITDATE, a.IDUSERCREATE, a.IDUSEREDIT, a.LINKED, a.STATUS  "
            + "        , m.NAME MENUNAME, CASE WHEN m.ISPAGE = 1 AND m.ISACTIVE = 1 THEN 'PAGE' WHEN m.ISPAGE = 0 AND m.ISACTIVE = 1 THEN 'ARTICLE' ELSE '' END TYPEMENU " 
            + " FROM Articles a LEFT JOIN Article_Menu am ON am.ARTICLEID = a.ID LEFT JOIN Menu m ON m.Id = am.MENUID"
            + "        ORDER BY a.LATESTEDITDATE DESC ";
            conn.Open();
            var result = await conn.QueryAsync<dynamic>(sQuery);
            return result;
        }

    } 


    [HttpGet("getusage")]
    // [AuthorizeAttribute]
    public async Task<dynamic> getusage()
    {

        var process = Process.GetCurrentProcess();
        var cpuUsage = process.TotalProcessorTime;
        var ramUsage = process.PrivateMemorySize64;
            var hostName = Environment.MachineName;
            var domainName = Environment.UserDomainName;

        var result = new {hostName, domainName, cpuUsage, ramUsage};
        return result;
    } 


}
