using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;

namespace Project.UseCases.Article
{
    public class LinkArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public string? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class LinkArticleCommand : IRequest<LinkArticleResponse>
    {
        public int SourceArticle { get; set; }
        public string? SourceArticleLang { get; set; }
        public int LinkArticle { get; set; }
        public string? LinkArticleLang { get; set; }
        public string? TypeLink { get; set; }
    }
    public class LinkArticleValidator : AbstractValidator<AddArticleCommand>
    {
        public LinkArticleValidator()
        {
        }
    }
    public class LinkArticleHandler : IRequestHandler<LinkArticleCommand, LinkArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public LinkArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<LinkArticleResponse> Handle(LinkArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    Project.Models.Article_Link _Link_Article_to_add = new Project.Models.Article_Link();
                    Project.Models.Articles _Link_Article = new Project.Models.Articles();
                    _Link_Article_to_add.SOURCEARTICLE = command.SourceArticle;
                    _Link_Article_to_add.SOURCEARTICLELANG = command.SourceArticleLang;
                    _Link_Article_to_add.LINKARTICLE = command.LinkArticle;
                    _Link_Article_to_add.LINKARTICLELANG = command.LinkArticleLang;
                    _dbContext.Add(_Link_Article_to_add);
                    _dbContext.SaveChanges();

                    if (command.TypeLink == "linkPage" || command.TypeLink == "linkArticle")
                    {
                        var sourceArticle = _dbContext.Articles.Where(x => x.ID == command.SourceArticle).FirstOrDefault();
                        sourceArticle.LINKED = 1;
                        _dbContext.Articles.Update(sourceArticle);
                        _dbContext.SaveChanges();

                        var linkArticle = _dbContext.Articles.Where(x => x.ID == command.LinkArticle).FirstOrDefault();
                        linkArticle.LINKED = 1;
                        _dbContext.Articles.Update(linkArticle);
                        _dbContext.SaveChanges();
                    }
                    // else if (command.TypeLink == "linkBlog")
                    // {
                    //     var sourceArticle = _dbContext.Blogs.Where(x => x.ID == command.SourceArticle).FirstOrDefault();
                    //     sourceArticle.LINKED = 1;
                    //     _dbContext.Blogs.Update(sourceArticle);
                    //     _dbContext.SaveChanges();

                    //     var linkArticle = _dbContext.Blogs.Where(x => x.ID == command.LinkArticle).FirstOrDefault();
                    //     linkArticle.LINKED = 1;
                    //     _dbContext.Blogs.Update(linkArticle);
                    //     _dbContext.SaveChanges();
                    // }

                    dbContextTransaction.Commit();
                    return new LinkArticleResponse
                    {
                        MESSAGE = "LINK_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new LinkArticleResponse
                    {
                        MESSAGE = "LINK_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
