using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.Blog
{
    public class DeleteBlogResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
    }
    public class DeleteBlogCommand : IRequest<DeleteBlogResponse>
    {
        public int ID { get; set; }
    }
    public class DeleteBlogValidator : AbstractValidator<DeleteBlogCommand>
    {
        public DeleteBlogValidator()
        {
        }
    }
    public class DeleteBlogHandler : IRequestHandler<DeleteBlogCommand, DeleteBlogResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public DeleteBlogHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<DeleteBlogResponse> Handle(DeleteBlogCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var blog_to_delete = _dbContext.Blogs.Where(x => x.ID == command.ID).FirstOrDefault();
                    var iduser = _accessor.HttpContext?.Items["ID"]?.ToString();
                    var _iduser = Int32.Parse(iduser);
                    var user = _dbContext.Users.Where(x => x.ID == _iduser).ToList();
                    var userRole = user.First().ROLE;
                    if (blog_to_delete.IDUSER.ToString() == iduser)
                    {
                        _dbContext.Blogs.Remove(_dbContext.Blogs.Find(command.ID));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteBlogResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                        };
                    }
                    else
                    {
                        return new DeleteBlogResponse
                        {
                            MESSAGE = "DELETE_FAIL_NOT_BLOG_OWNER",
                            STATUSCODE = HttpStatusCode.InternalServerError,
                        };
                    }

                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new DeleteBlogResponse
                    {
                        MESSAGE = "DELETE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }

        }
    }
}
