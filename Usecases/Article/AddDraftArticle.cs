using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;

namespace Project.UseCases.Article
{
    public class AddDraftArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public ArticleDto? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddDraftArticleCommand : IRequest<AddDraftArticleResponse>
    {
        public string? Avatar { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Hastag { get; set; }
        public IEnumerable<int>? Menu { get; set; }
        public string? Language { get; set; }
        public string? ArticleContent { get; set; }
        public string? Slug { get; set; }
        public string? Page { get; set; }
    }
    public class AddDraftArticleValidator : AbstractValidator<AddDraftArticleCommand>
    {
        public AddDraftArticleValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("TITLE CANNOT BE EMPTY");
            RuleFor(x => x.ArticleContent).NotNull().NotEmpty().WithMessage("ARTICLE CONTENT CANNOT BE EMPTY");
            RuleFor(x => x.Hastag).NotNull().NotEmpty().WithMessage("HASTAG CANNOT BE EMPTY");
            RuleFor(x => x.Avatar).NotNull().NotEmpty().WithMessage("AVATAR CANNOT BE EMPTY");
            RuleFor(x => x.Language).NotNull().NotEmpty().WithMessage("LANGUAGE CANNOT BE EMPTY");
            RuleFor(x => x.Menu).NotNull().NotEmpty().WithMessage("MENU CANNOT BE EMPTY");
        }
    }
    public class AddDraftArticleHandler : IRequestHandler<AddDraftArticleCommand, AddDraftArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public AddDraftArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<AddDraftArticleResponse> Handle(AddDraftArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    bool _slug_checked = _dbContext.Articles.Any(u => u.SLUG == command.Slug);

                    List<string> _existed_prop = new List<string> { _slug_checked ? "SLUG_ALREADY_EXISTS" : string.Empty };
                    _existed_prop.RemoveAll(s => s == string.Empty);
                    if (_existed_prop.Count() > 0)
                    {
                        return new AddDraftArticleResponse
                        {
                            MESSAGE = "ADD_FAIL",
                            STATUSCODE = HttpStatusCode.InternalServerError,
                            ERROR = _existed_prop
                        };
                    }
                    else
                    {
                        Project.Models.Articles _Article_to_add = _mapper.Map<Project.Models.Articles>(command);
                        Project.Models.Article_Menu _Article_Menu_to_add = new Project.Models.Article_Menu();
                        _Article_to_add.CREATEDATE = DateTime.Now;
                        _Article_to_add.LATESTEDITDATE = DateTime.Now;
                        _Article_to_add.PRIORITYLEVEL = 1;
                        _Article_to_add.IDUSERCREATE = Int32.Parse(iduser);
                        _Article_to_add.STATUS = "DRAFT";
                        _dbContext.Add(_Article_to_add);
                        _dbContext.SaveChanges();
                        _dbContext.Entry(_Article_to_add).GetDatabaseValues();
                        var id = _Article_to_add.ID;
                        foreach (int menuID in command.Menu)
                        {
                            _Article_Menu_to_add.MENUID = menuID;
                            _Article_Menu_to_add.ARTICLEID = id;
                            _dbContext.Add(_Article_Menu_to_add);
                            _dbContext.SaveChanges();

                        }

                        dbContextTransaction.Commit();
                        return new AddDraftArticleResponse
                        {
                            MESSAGE = "ADD_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<ArticleDto>(_Article_to_add)
                        };
                    }

                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddDraftArticleResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
