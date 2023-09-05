using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Project.UseCases.Article
{
    public class GetArticleContentResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
    }
    public class GetArticleContentCommand : IRequest<GetArticleContentResponse>
    {
        public int ID { get; init; }
    }
    public class GetArticleContentValidator : AbstractValidator<GetArticleContentCommand>
    {
        public GetArticleContentValidator()
        {
            //RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("QUERY TYPE CANNOT BE EMPTY");
            //RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("QUERY DATA CANNOT BE EMPTY");
        }
    }
    public class GetArticleContentHandler : IRequestHandler<GetArticleContentCommand, GetArticleContentResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public GetArticleContentHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<GetArticleContentResponse> Handle(GetArticleContentCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.Articles> list_Article_response = Enumerable.Empty<Project.Models.Articles>();
                IEnumerable<Project.Models.Article_Menu> list_Article_Menu_response = Enumerable.Empty<Project.Models.Article_Menu>();
                IEnumerable<Project.Models.Articles> result = Enumerable.Empty<Project.Models.Articles>();

                var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                var _iduser = Int32.Parse(iduser);
                var rolcode = _dbContext.Users.Where(x => x.ID == _iduser).Select(x => x.ROLE).FirstOrDefault();
                var listmenu = _dbContext.Role_Menu.Where(x2 => x2.ROLECODE == rolcode).Select(x2 => x2.MENUID).ToList();


                if (command.ID == 0)
                {
                    return new GetArticleContentResponse
                    {
                        MESSAGE = "GET_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                var result2 = from arc in _dbContext.Articles.ToList()
                              where arc.ID == command.ID
                              select new { arc.ARTICLECONTENT };
                return new GetArticleContentResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = result2,

                };
            }
            catch
            {
                return new GetArticleContentResponse
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