using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;

namespace Project.UseCases.ListItem
{
    public class AddListItemResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddListItemCommand : IRequest<AddListItemResponse>
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
    public class AddListItemValidator : AbstractValidator<AddListItemCommand>
    {
        public AddListItemValidator()
        {

        }
    }
    public class AddListItemHandler : IRequestHandler<AddListItemCommand, AddListItemResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _accessor;

        public AddListItemHandler(DataContext dbContext, IMapper mapper, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _accessor = accessor;
        }
        public async Task<AddListItemResponse> Handle(AddListItemCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (command.Type == "position")
                    {
                        GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                        Project.Models.ListPosition _ListItem_to_add = _mapper.Map<Project.Models.ListPosition>(command);
                        _dbContext.Add(_ListItem_to_add);
                        _dbContext.SaveChanges();
                    }
                    else if (command.Type == "title")
                    {
                        GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                        Project.Models.ListTitle _ListItem_to_add = _mapper.Map<Project.Models.ListTitle>(command);
                        _dbContext.Add(_ListItem_to_add);
                        _dbContext.SaveChanges();
                    }
                    else if (command.Type == "department")
                    {
                        GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                        Project.Models.ListDepartment _ListItem_to_add = _mapper.Map<Project.Models.ListDepartment>(command);
                        _dbContext.Add(_ListItem_to_add);
                        _dbContext.SaveChanges();
                    }


                    dbContextTransaction.Commit();
                    return new AddListItemResponse
                    {
                        MESSAGE = "ADD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddListItemResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
