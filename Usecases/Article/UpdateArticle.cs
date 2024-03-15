using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Project.UseCases.Article
{
    public class UpdateArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public ArticleDto? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class UpdateArticleCommand : IRequest<UpdateArticleResponse>
    {
        public int ID { get; set; }
        public string? Avatar { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Hastag { get; set; }
        public IEnumerable<int>? Menu { get; set; }
        public string? Language { get; set; }
        public string? Article_Content { get; set; }
        public string? Slug { get; set; }
        public string? Page { get; set; }
    }
    public class UpdateArticleValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, UpdateArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public UpdateArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<UpdateArticleResponse> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    bool _slug_checked = _dbContext.Articles.Any(u => u.SLUG == command.Slug);
                    Project.Models.Articles _article_check = _dbContext.Articles.AsNoTracking().FirstOrDefault(x => x.SLUG == command.Slug);

                    List<string> _existed_prop = new List<string> { _slug_checked ? "SLUG_ALREADY_EXISTS" : string.Empty };
                    _existed_prop.RemoveAll(s => s == string.Empty);
                    if (_existed_prop.Count() > 0 && _article_check.ID != command.ID)
                    {
                        return new UpdateArticleResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.InternalServerError,
                            ERROR = _existed_prop
                        };
                    }
                    else
                    {
                        Project.Models.Articles? _Article_to_update = await _dbContext.Articles.AsNoTracking().FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                        if (_Article_to_update != null)
                        {
                            _mapper.Map<UpdateArticleCommand, Project.Models.Articles>(command, _Article_to_update);
                            _Article_to_update.LATESTEDITDATE = DateTime.Now;
                            _Article_to_update.IDUSEREDIT = Int32.Parse(iduser);
                            _dbContext.Articles.Update(_Article_to_update);
                            _dbContext.SaveChanges();

                            _dbContext.Article_Menu.RemoveRange(_dbContext.Article_Menu.Where(x => x.ARTICLEID == command.ID));
                            _dbContext.SaveChanges();

                            Project.Models.Article_Menu _Article_Menu_to_add = new Project.Models.Article_Menu();
                            foreach (var menu in command.Menu)
                            {
                                _Article_Menu_to_add.MENUID = menu;
                                _Article_Menu_to_add.ARTICLEID = command.ID;
                                await _dbContext.Article_Menu.AddAsync(_Article_Menu_to_add);
                                _dbContext.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                            return new UpdateArticleResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _mapper.Map<ArticleDto>(_Article_to_update)
                            };
                        }
                        else
                        {
                            return new UpdateArticleResponse
                            {
                                MESSAGE = "UPDATE_FAIL",
                                STATUSCODE = HttpStatusCode.BadRequest
                            };
                        }
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateArticleResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}