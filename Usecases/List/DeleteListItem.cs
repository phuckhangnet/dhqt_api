using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Project.UseCases.ListItem
{
    public class DeleteListItemResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
    }
    public class DeleteListItemCommand : IRequest<DeleteListItemResponse>
    {
        public string? Code { get; set; }
        public string? Type { get; set; }
    }
    public class DeleteListItemValidator : AbstractValidator<DeleteListItemCommand>
    {
        public DeleteListItemValidator()
        {
        }
    }
    public class DeleteListItemHandler : IRequestHandler<DeleteListItemCommand, DeleteListItemResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public DeleteListItemHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<DeleteListItemResponse> Handle(DeleteListItemCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (command.Type == "position")
                    {
                        _dbContext.ListPosition.Remove(_dbContext.ListPosition.Find(command.Code));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteListItemResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                        };
                    }
                    else if (command.Type == "title")
                    {
                        _dbContext.ListTitle.Remove(_dbContext.ListTitle.Find(command.Code));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteListItemResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                        };
                    }
                    else if (command.Type == "department")
                    {
                        _dbContext.ListDepartment.Remove(_dbContext.ListDepartment.Find(command.Code));
                        _dbContext.SaveChanges();
                        dbContextTransaction.Commit();
                        return new DeleteListItemResponse
                        {
                            MESSAGE = "DELETE_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                        };
                    }
                    return new DeleteListItemResponse
                    {
                        MESSAGE = "DELETE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError,
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new DeleteListItemResponse
                    {
                        MESSAGE = "DELETE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }

        }
    }
}
