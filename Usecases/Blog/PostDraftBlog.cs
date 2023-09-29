using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.UseCases.Blog
{
    public class PostDraftBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public BlogDto? RESPONSES { get; set; }
    }
    public class PostDraftBlogCommand : IRequest<PostDraftBlogResponse>
    {
        public int? ID { get; set; }
    }
    public class PostDraftBlogValidator : AbstractValidator<PostDraftBlogCommand>
    {
        public PostDraftBlogValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class PostDraftBlogHandler : IRequestHandler<PostDraftBlogCommand, PostDraftBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public PostDraftBlogHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<PostDraftBlogResponse> Handle(PostDraftBlogCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Blogs? _Blog_to_PostDraft = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Blog_to_PostDraft != null)
                    {
                        _Blog_to_PostDraft.LATESTEDITDATE = DateTime.Now;
                        _Blog_to_PostDraft.STATUS = "POSTED";
                        _dbContext.Blogs.Update(_Blog_to_PostDraft);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new PostDraftBlogResponse
                        {
                            MESSAGE = "UPDATE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<BlogDto>(_Blog_to_PostDraft)
                        };
                    }
                    else
                    {
                        return new PostDraftBlogResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new PostDraftBlogResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
