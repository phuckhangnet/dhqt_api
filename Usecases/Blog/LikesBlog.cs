using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.UseCases.Blog
{
    public class LikesBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public BlogDto? RESPONSES { get; set; }
    }
    public class LikesBlogCommand : IRequest<LikesBlogResponse>
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public string? Article_Content { get; set; }
        public string? Hastag { get; set; }
        public string? Language { get; set; }
    }
    public class LikesBlogValidator : AbstractValidator<LikesBlogCommand>
    {
        public LikesBlogValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
        }
    }
    public class LikesBlogHandler : IRequestHandler<LikesBlogCommand, LikesBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public LikesBlogHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<LikesBlogResponse> Handle(LikesBlogCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Blogs? _Blog_to_likes = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Blog_to_likes != null)
                    {
                        _mapper.Map<LikesBlogCommand, Project.Models.Blogs>(command, _Blog_to_likes);
                        _Blog_to_likes.LIKES += 1;
                        _dbContext.Blogs.Update(_Blog_to_likes);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new LikesBlogResponse
                        {
                            MESSAGE = "LIKE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<BlogDto>(_Blog_to_likes)
                        };
                    }
                    else
                    {
                        return new LikesBlogResponse
                        {
                            MESSAGE = "LIKE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new LikesBlogResponse
                    {
                        MESSAGE = "LIKE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
