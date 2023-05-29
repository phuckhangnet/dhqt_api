using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.UseCases.Blog
{
    public class UpdateBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public BlogDto? RESPONSES { get; set; }
    }
    public class UpdateBlogCommand : IRequest<UpdateBlogResponse>
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public string? Article_Content { get; set; }
        public string? Hastag { get; set; }
        public string? Language { get; set; }
    }
    public class UpdateBlogValidator : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class UpdateBlogHandler : IRequestHandler<UpdateBlogCommand, UpdateBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public UpdateBlogHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<UpdateBlogResponse> Handle(UpdateBlogCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Blogs? _Blog_to_update = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Blog_to_update != null)
                    {
                        _mapper.Map<UpdateBlogCommand, Project.Models.Blogs>(command, _Blog_to_update);
                        _Blog_to_update.LATESTEDITDATE = DateTime.Now;
                        _dbContext.Blogs.Update(_Blog_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new UpdateBlogResponse
                        {
                            MESSAGE = "UPDATE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<BlogDto>(_Blog_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateBlogResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateBlogResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
