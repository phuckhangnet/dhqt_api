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
    public class PostDraftArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public ArticleDto? RESPONSES { get; set; }
    }
    public class PostDraftArticleCommand : IRequest<PostDraftArticleResponse>
    {
        public int ID { get; set; }
    }
    public class PostDraftArticleValidator : AbstractValidator<PostDraftArticleCommand>
    {
        public PostDraftArticleValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class PostDraftArticleHandler : IRequestHandler<PostDraftArticleCommand, PostDraftArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public PostDraftArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<PostDraftArticleResponse> Handle(PostDraftArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    Project.Models.Articles? _Article_to_update = await _dbContext.Articles.AsNoTracking().FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Article_to_update != null)
                    {
                        _Article_to_update.LATESTEDITDATE = DateTime.Now;
                        _Article_to_update.IDUSEREDIT = Int32.Parse(iduser);
                        _Article_to_update.STATUS = "POSTED";
                        _dbContext.Articles.Update(_Article_to_update);
                        _dbContext.SaveChanges();

                        dbContextTransaction.Commit();
                        return new PostDraftArticleResponse
                        {
                            MESSAGE = "UPDATE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<ArticleDto>(_Article_to_update)
                        };
                    }
                    else
                    {
                        return new PostDraftArticleResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new PostDraftArticleResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
