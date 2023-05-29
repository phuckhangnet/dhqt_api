using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListItem
{
    public class UpdateListItemResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
    }
    public class UpdateListItemCommand : IRequest<UpdateListItemResponse>
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
    }
    public class UpdateListItemValidator : AbstractValidator<UpdateListItemCommand>
    {
        public UpdateListItemValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("ID không được trống");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateListItemHandler : IRequestHandler<UpdateListItemCommand, UpdateListItemResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateListItemHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateListItemResponse> Handle(UpdateListItemCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (command.Type == "position")
                    {
                        Project.Models.ListPosition? _ListItem_to_update = await _dbContext.ListPosition.FirstOrDefaultAsync(x => x.CODE == command.Code, cancellationToken);
                        if (_ListItem_to_update != null)
                        {
                            _mapper.Map<UpdateListItemCommand, Project.Models.ListPosition>(command, _ListItem_to_update);
                            _dbContext.ListPosition.Update(_ListItem_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            dbContextTransaction.Commit();
                            return new UpdateListItemResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _ListItem_to_update
                            };
                        }
                    }
                    else if (command.Type == "title")
                    {
                        Project.Models.ListTitle? _ListItem_to_update = await _dbContext.ListTitle.FirstOrDefaultAsync(x => x.CODE == command.Code, cancellationToken);
                        if (_ListItem_to_update != null)
                        {
                            _mapper.Map<UpdateListItemCommand, Project.Models.ListTitle>(command, _ListItem_to_update);
                            _dbContext.ListTitle.Update(_ListItem_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            dbContextTransaction.Commit();
                            return new UpdateListItemResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _ListItem_to_update
                            };
                        }
                    }
                    else if (command.Type == "department")
                    {
                        Project.Models.ListDepartment? _ListItem_to_update = await _dbContext.ListDepartment.FirstOrDefaultAsync(x => x.CODE == command.Code, cancellationToken);
                        if (_ListItem_to_update != null)
                        {
                            _mapper.Map<UpdateListItemCommand, Project.Models.ListDepartment>(command, _ListItem_to_update);
                            _dbContext.ListDepartment.Update(_ListItem_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            dbContextTransaction.Commit();
                            return new UpdateListItemResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _ListItem_to_update
                            };
                        }
                    }
                    else
                    {
                        return new UpdateListItemResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                    return new UpdateListItemResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateListItemResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
