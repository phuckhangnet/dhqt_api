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
    public class UpdateNoMenuArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public ArticleDto? RESPONSES { get; set; }
    }
    public class UpdateNoMenuArticleCommand : IRequest<UpdateNoMenuArticleResponse>
    {
        public int ID { get; set; }
        public string? Avatar { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Hastag { get; set; }
        public string? Language { get; set; }
        public string? Article_Content { get; set; }
    }
    public class UpdateNoMenuArticleValidator : AbstractValidator<UpdateNoMenuArticleCommand>
    {
        public UpdateNoMenuArticleValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class UpdateNoMenuArticleHandler : IRequestHandler<UpdateNoMenuArticleCommand, UpdateNoMenuArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public UpdateNoMenuArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<UpdateNoMenuArticleResponse> Handle(UpdateNoMenuArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    Project.Models.Articles? _Article_to_update = await _dbContext.Articles.AsNoTracking().FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Article_to_update != null)
                    {
                        _mapper.Map<UpdateNoMenuArticleCommand, Project.Models.Articles>(command, _Article_to_update);
                        _Article_to_update.LATESTEDITDATE = DateTime.Now;
                        _Article_to_update.IDUSEREDIT = Int32.Parse(iduser);
                        _dbContext.Articles.Update(_Article_to_update);
                        _dbContext.SaveChanges();

                        dbContextTransaction.Commit();
                        return new UpdateNoMenuArticleResponse
                        {
                            MESSAGE = "UPDATE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<ArticleDto>(_Article_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateNoMenuArticleResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateNoMenuArticleResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
