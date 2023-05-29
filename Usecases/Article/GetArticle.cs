using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.Article
{
    public class GetArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<ArticleDto>? RESPONSES { get; set; }
        public dynamic? articlemenu { get; set; }
    }
    public class GetArticleCommand : IRequest<GetArticleResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
        public int NoOfResult { get; set; }
        public IEnumerable<int>? MenuID { get; set; } = Enumerable.Empty<int>();
        public string? CurrentRole { get; set; }
    }
    public class GetArticleValidator : AbstractValidator<GetArticleCommand>
    {
        public GetArticleValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("QUERY TYPE CANNOT BE EMPTY");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("QUERY DATA CANNOT BE EMPTY");
        }
    }
    public class GetArticleHandler : IRequestHandler<GetArticleCommand, GetArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public GetArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<GetArticleResponse> Handle(GetArticleCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.Articles> list_Article_response = Enumerable.Empty<Project.Models.Articles>();
                IEnumerable<Project.Models.Article_Menu> list_Article_Menu_response = Enumerable.Empty<Project.Models.Article_Menu>();
                IEnumerable<Project.Models.Articles> result = Enumerable.Empty<Project.Models.Articles>();
                if (command.Type == null)
                {
                    return new GetArticleResponse
                    {
                        MESSAGE = "MISSING_PARAMETER_TYPE",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                else if (command.Type != "GET_ALL" && command.Type != "GET_ALL_FROM_USER" && command.Type != "GET_BY_ID" && command.Type != "GET_BY_HASTAG" && command.Type != "GET_BY_MENU_ID")
                {
                    return new GetArticleResponse
                    {
                        MESSAGE = "INVALID_PARAMETER_TYPE",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                else if (command.Type == "GET_ALL")
                {

                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    var _iduser = Int32.Parse(iduser);
                    var listmenu = _dbContext.Role_Menu.Where(x2 => x2.ROLECODE == command.CurrentRole).Select(x2 => x2.MENUID).ToList();
                    if (command.MenuID.Count() > 0)
                    {
                        listmenu = listmenu.Where(x => command.MenuID.Contains(x)).ToList();
                    }
                    var Article_Menu = _dbContext.Article_Menu.Where(x => listmenu.Contains(x.MENUID))
                    .Select(x => new { ARTICLEID = x.ARTICLEID, MENUID = new List<int>(), MENUNAME = new List<string>() }).Distinct().ToList();
                    Article_Menu.ForEach(x =>
                    {
                        var menuids = _dbContext.Article_Menu.Where(x2 => x2.ARTICLEID == x.ARTICLEID).Select(x2 => x2.MENUID).ToList();
                        x.MENUID.AddRange(menuids);
                        var menunames = _dbContext.Menu.Where(x2 => menuids.Contains(x2.ID)).Select(x2 => x2.NAME).ToList();
                        x.MENUNAME.AddRange(menunames);
                    });
                    var result2 = from arc in _dbContext.Articles.ToList()
                                  join arcme in Article_Menu on arc.ID equals arcme.ARTICLEID
                                  join urs in _dbContext.Users.ToList() on arc.IDUSERCREATE equals urs.ID
                                  join ursdetail in _dbContext.User_Detail.ToList() on urs.ID equals ursdetail.USERID
                                  orderby arc.CREATEDATE descending
                                  select new { arc, arcme.MENUID, arcme.MENUNAME, ursdetail.NAME };
                    return new GetArticleResponse
                    {
                        MESSAGE = "GET_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        articlemenu = result2,
                    };
                }
                else if (command.Type == "GET_ALL_FROM_USER")
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    var _iduser = Int32.Parse(iduser);
                    var rolcode = _dbContext.Users.Where(x => x.ID == _iduser).Select(x => x.ROLE).FirstOrDefault();
                    var listmenu = _dbContext.Role_Menu.Where(x2 => x2.ROLECODE == rolcode).Select(x2 => x2.MENUID).ToList();
                    if (command.MenuID.Count() > 0)
                    {
                        listmenu = listmenu.Where(x => command.MenuID.Contains(x)).ToList();
                    }
                    var Article_Menu = _dbContext.Article_Menu.Where(x => listmenu.Contains(x.MENUID))
                    .Select(x => new { ARTICLEID = x.ARTICLEID, MENUID = new List<int>(), MENUNAME = new List<string>() }).Distinct().ToList();
                    Article_Menu.ForEach(x =>
                    {
                        var menuids = _dbContext.Article_Menu.Where(x2 => x2.ARTICLEID == x.ARTICLEID).Select(x2 => x2.MENUID).ToList();
                        x.MENUID.AddRange(menuids);
                        var menunames = _dbContext.Menu.Where(x2 => menuids.Contains(x2.ID)).Select(x2 => x2.NAME).ToList();
                        x.MENUNAME.AddRange(menunames);
                    });
                    var result2 = from arc in _dbContext.Articles.ToList()
                                  join arcme in Article_Menu on arc.ID equals arcme.ARTICLEID
                                  join urs in _dbContext.Users.ToList() on arc.IDUSERCREATE equals urs.ID
                                  join ursdetail in _dbContext.User_Detail.ToList() on urs.ID equals ursdetail.USERID
                                  where arc.IDUSERCREATE == _iduser
                                  orderby arc.CREATEDATE descending
                                  select new { arc, arcme.MENUID, arcme.MENUNAME, ursdetail.NAME };
                    return new GetArticleResponse
                    {
                        MESSAGE = "GET_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        //RESPONSES = _mapper.Map<IEnumerable<ArticleDto>>(result)
                        articlemenu = result2,
                    };
                }
                else
                {
                    if (command.Data.Count() == 0)
                    {
                        return new GetArticleResponse
                        {
                            MESSAGE = "MISSING_PARAMETER_DATA",
                            STATUSCODE = HttpStatusCode.InternalServerError
                        };
                    }
                    else
                    {
                        switch (command.Type)
                        {
                            case "GET_BY_HASTAG":
                                foreach (string _data in command.Data)
                                {
                                    var listItem = _data.Split(",");
                                    list_Article_response = (from a in _dbContext.Articles.AsEnumerable()
                                                             where checkArray(listItem, a.HASTAG.Split(",")) == true
                                                             orderby a.CREATEDATE
                                                             select a);
                                    result = result.Concat(list_Article_response).Distinct();
                                    //orderby t.CREATE_DATE).Take(command.No_of_result);
                                    //list_Article_response = await _dbContext.Articles.ToListAsync(cancellationToken);
                                    // foreach (Project.Models.Articles article in list_Article_response)
                                    // {
                                    //     var listHastag = article.HASTAG.Split(",");
                                    //     for (int i = 0; i < listItem.Length; i++)
                                    //     {
                                    //         if (listHastag.ToList().Contains(listItem[i]))
                                    //         {
                                    //             if (!list_Article_Pass.Contains(article))
                                    //             {
                                    //                 list_Article_Pass.Add(article);
                                    //             }
                                    //         }
                                    //         else
                                    //         {
                                    //             if (list_Article_Pass.Contains(article))
                                    //             {
                                    //                 list_Article_Pass.Remove(article);
                                    //             }
                                    //             break;
                                    //         }
                                    //     }
                                    // }
                                    // result = result.Concat((from t in list_Article_Pass
                                    //                         orderby t.CREATE_DATE
                                    //                         select t).Take(command.No_of_result));
                                }
                                result = result.Take(command.NoOfResult);
                                break;
                            case "GET_BY_ID":
                                result = await _dbContext.Articles.Where(x => command.Data.Contains(x.ID.ToString())).ToListAsync(cancellationToken);
                                break;
                                // case "GET_BY_MENU_ID":
                                //     foreach (string _data in command.Data)
                                //     {
                                //         var listItem = _data.Split(",");
                                //         list_Article_Menu_response = (from a in _dbContext.Article_Menu.AsEnumerable()
                                //                                       where checkArray(listItem, a.MENUID.Split(",")) == true
                                //                                       select a);
                                //         foreach (var _response in list_Article_Menu_response.ToList())
                                //         {
                                //             //result = await _dbContext.Articles.Where(x => x.ID == _response.ARTICLEID).ToListAsync(cancellationToken);
                                //             result = result.Concat(await _dbContext.Articles.Where(x => x.ID == _response.ARTICLEID).ToListAsync(cancellationToken)).Distinct();
                                //         }
                                //         // if (listItem.Length == 1)
                                //         // {
                                //         //     list_Article_Menu_response = await _dbContext.Article_Menu.Where(x => x.MENUID.ToString() == listItem[0]).ToListAsync(cancellationToken);
                                //         //     foreach (Project.Models.Article_Menu response in list_Article_Menu_response)
                                //         //     {
                                //         //         list_Article_response = await _dbContext.Articles.Where(x => x.ID == response.ARTICLEID).ToListAsync(cancellationToken);
                                //         //         result = result.Concat((from t in list_Article_response
                                //         //                                 orderby t.CREATEDATE
                                //         //                                 select t).Take(command.NoOfResult));
                                //         //     }
                                //         // }
                                //         // else
                                //         // {
                                //         //     list_Article_Menu_response = await _dbContext.Article_Menu.Where(x => x.MENUID.ToString() == listItem[0]).ToListAsync(cancellationToken);
                                //         //     foreach (Project.Models.Article_Menu response in list_Article_Menu_response)
                                //         //     {
                                //         //         for (int i = 1; i < listItem.Length; i++)
                                //         //         {
                                //         //             var list_Article_Menu_response_ = await _dbContext.Article_Menu.Where(x => x.MENUID.ToString() == listItem[i] && x.ARTICLEID == response.ARTICLEID).ToListAsync(cancellationToken);
                                //         //             if (list_Article_Menu_response_.Count() == 0)
                                //         //             {
                                //         //                 break;
                                //         //             }
                                //         //             list_Article_response = await _dbContext.Articles.Where(x => x.ID == response.ARTICLEID).ToListAsync(cancellationToken);
                                //         //             result = result.Concat((from t in list_Article_response
                                //         //                                     orderby t.CREATEDATE
                                //         //                                     select t).Take(command.NoOfResult));
                                //         //         }

                                //         //     }
                                //         // }
                                //     }
                                //     result = result.Take(command.NoOfResult);
                                //     break;
                        }
                    }
                    return new GetArticleResponse
                    {
                        MESSAGE = "GET_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<IEnumerable<ArticleDto>>(result)
                    };
                }
            }
            catch
            {
                return new GetArticleResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }

        private bool checkArray(string[] a, string[] b)
        {
            a = a.Distinct().ToArray();
            b = b.Distinct().ToArray();
            var c = a.Concat(b).ToArray();
            var dup = c.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Count();
            if (dup == a.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
