using Microsoft.AspNetCore.Mvc;
using MediatR;
using Project.UseCases.Menu;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Data;

namespace ProjectBE.Controllers;
[Route("menu")]
public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;
    private DataContext _context;

    public MenuController(ILogger<MenuController> logger, IMediator mediator, IConfiguration config, DataContext context)
    {
        _logger = logger;
        _mediator = mediator;
        _config = config;
        _context = context;
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

    [HttpGet("GetChangeLanguagePage")]
    public async Task<string> GetChangeLanguagePage(string menuid, string postid, string lang)
    {
        Article_Menu am = _context.Article_Menu.Where(x => x.MENUID.ToString() == menuid).FirstOrDefault();

        string url = "";

        if (!string.IsNullOrEmpty(postid) && postid != "0") 
        {
            if (am != null)
            {
                string slug = _context.Menu.Where(x => x.ID == am.MENUID).FirstOrDefault().SLUG;

                string sub = _context.Articles.Where(x => x.HASTAG.Trim().Replace("#","") == slug && x.LANGUAGE != lang).FirstOrDefault().SLUG;

                Article_Link al = _context.Article_Link.Where(x => x.SOURCEARTICLE.ToString() == postid || x.LINKARTICLE.ToString() == postid).FirstOrDefault();

                string postsl = "";
                if (al != null)
                {
                    if (al.SOURCEARTICLE.ToString() == postid)
                        postsl = _context.Articles.Where(x => x.ID == al.LINKARTICLE).First().SLUG;
                    else
                        postsl = _context.Articles.Where(x => x.ID == al.SOURCEARTICLE).First().SLUG;

                    url = sub + "/" + postsl;

                }


            }
        }
        else
        {
            if (am != null)
            {
                string slug = _context.Menu.Where(x => x.ID == am.MENUID).FirstOrDefault().SLUG;

                var a = _context.Articles.Where(x => x.HASTAG.Trim().Replace("#","") == slug && x.LANGUAGE != lang).FirstOrDefault();

                if (a != null) { url = a.SLUG; }

            }
        }

        return url;
    }


    [HttpGet("GetMenuById")]
    public async Task<dynamic> GetMenuById(int id)
    {
        return _context.Menu.Where(x => x.ID == id).FirstOrDefault();

    }

    [HttpGet("GetMenuBySlug")]
    public async Task<dynamic> GetMenuBySlug(string slug)
    {
        Menu m = _context.Menu.Where(x => x.SLUG.Trim() == slug.Trim()).FirstOrDefault();
        return m;

    }

    [HttpGet("GetMenuByDescription")]
    public async Task<dynamic> GetMenuByDescription(string description)
    {
        return _context.Menu.Where(x => x.DESCRIPTION.Contains(description)).FirstOrDefault();

    }

    [HttpGet("GetMenuByLevelAndParent")]
    public async Task<dynamic> GetMenuByLevelAndParent(string page, int level, string parent)
    {
        return _context.Menu.Where(x => x.PAGE == page && x.MENULEVEL == level && x.PARENT == parent.Trim()).FirstOrDefault();

    }

    

}
