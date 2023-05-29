using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.Article
{
    public class DeleteArticleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
    }
    public class DeleteArticleCommand : IRequest<DeleteArticleResponse>
    {
        public int ID { get; set; }
    }
    public class DeleteArticleValidator : AbstractValidator<DeleteArticleCommand>
    {
        public DeleteArticleValidator()
        {
        }
    }
    public class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, DeleteArticleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public DeleteArticleHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<DeleteArticleResponse> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var article_to_delete = _dbContext.Articles.Where(x => x.ID == command.ID).FirstOrDefault();
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    var _iduser = Int32.Parse(iduser);
                    var user = _dbContext.Users.Where(x => x.ID == _iduser).ToList();
                    var userRole = user.First().ROLE;
                    if (article_to_delete.IDUSERCREATE.ToString() == iduser || userRole == "ADMI" || userRole == "MADE")
                    {
                        _dbContext.Articles.Remove(_dbContext.Articles.Find(command.ID));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteArticleResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                        };
                    }
                    else
                    {
                        return new DeleteArticleResponse
                        {
                            MESSAGE = "DELETE_FAIL_NOT_ARTICLE_CREATOR",
                            STATUSCODE = HttpStatusCode.InternalServerError,
                        };
                    }

                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new DeleteArticleResponse
                    {
                        MESSAGE = "DELETE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }

        }
    }
}
