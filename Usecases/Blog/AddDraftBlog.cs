using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models;

namespace Project.UseCases.Blog
{
    public class AddDraftBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public BlogDto? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddDraftBlogCommand : IRequest<AddDraftBlogResponse>
    {
        public string? Title { get; set; }
        public string? ArticleContent { get; set; }
        public string? Hastag { get; set; }
        public string? Language { get; set; }
        public int IdUser { get; set; }
    }
    public class AddDraftBlogValidator : AbstractValidator<AddDraftBlogCommand>
    {
        public AddDraftBlogValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("TITLE CANNOT BE EMPTY");
            RuleFor(x => x.Language).NotNull().NotEmpty().WithMessage("LANGUAGE CANNOT BE EMPTY");
            RuleFor(x => x.IdUser).NotNull().NotEmpty().WithMessage("USER CANNOT BE EMPTY");
        }
    }
    public class AddDraftBlogHandler : IRequestHandler<AddDraftBlogCommand, AddDraftBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public AddDraftBlogHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<AddDraftBlogResponse> Handle(AddDraftBlogCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    Project.Models.Blogs _DraftBlog_to_add = _mapper.Map<Project.Models.Blogs>(command);
                    _DraftBlog_to_add.CREATEDATE = DateTime.Now;
                    _DraftBlog_to_add.LATESTEDITDATE = DateTime.Now;
                    _DraftBlog_to_add.IDUSER = Int32.Parse(iduser);
                    _DraftBlog_to_add.STATUS = "DRAFT";
                    _DraftBlog_to_add.LIKES = 0;
                    await _dbContext.AddAsync(_DraftBlog_to_add);
                    await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                    return new AddDraftBlogResponse
                    {
                        MESSAGE = "ADD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<BlogDto>(_DraftBlog_to_add)
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddDraftBlogResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
