using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;

namespace Project.UseCases.Article
{
    public class UnlinkArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public string? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class UnlinkArticleCommand : IRequest<UnlinkArticleResponse>
    {
        public int SourceArticle { get; set; }
        public string? TypeUnlink { get; set; }
    }
    public class UnlinkArticleValidator : AbstractValidator<AddArticleCommand>
    {
        public UnlinkArticleValidator()
        {
        }
    }
    public class UnlinkArticleHandler : IRequestHandler<UnlinkArticleCommand, UnlinkArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public UnlinkArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<UnlinkArticleResponse> Handle(UnlinkArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var article_link_to_delete = _dbContext.Article_Link.Where(x => x.SOURCEARTICLE == command.SourceArticle || x.LINKARTICLE == command.SourceArticle).FirstOrDefault();
                    if (command.TypeUnlink == "unlinkArticle" || command.TypeUnlink == "unlinkPage")
                    {
                        var sourceArticle = _dbContext.Articles.Where(x => x.ID == article_link_to_delete.SOURCEARTICLE).FirstOrDefault();
                        sourceArticle.LINKED = 0;
                        _dbContext.Articles.Update(sourceArticle);
                        _dbContext.SaveChanges();

                        var linkArticle = _dbContext.Articles.Where(x => x.ID == article_link_to_delete.LINKARTICLE).FirstOrDefault();
                        linkArticle.LINKED = 0;
                        _dbContext.Articles.Update(linkArticle);
                        _dbContext.SaveChanges();
                    }
                    // else if (command.TypeUnlink == "unlinkBlog")
                    // {
                    //     var sourceArticle = _dbContext.Blogs.Where(x => x.ID == article_link_to_delete.SOURCEARTICLE).FirstOrDefault();
                    //     sourceArticle.LINKED = 0;
                    //     _dbContext.Blogs.Update(sourceArticle);
                    //     _dbContext.SaveChanges();

                    //     var linkArticle = _dbContext.Blogs.Where(x => x.ID == article_link_to_delete.LINKARTICLE).FirstOrDefault();
                    //     linkArticle.LINKED = 0;
                    //     _dbContext.Blogs.Update(linkArticle);
                    //     _dbContext.SaveChanges();
                    // }

                    _dbContext.Article_Link.Remove(_dbContext.Article_Link.Find(article_link_to_delete.SOURCEARTICLE, article_link_to_delete.LINKARTICLE));
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    return new UnlinkArticleResponse
                    {
                        MESSAGE = "UNLINK_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UnlinkArticleResponse
                    {
                        MESSAGE = "UNLINK_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
