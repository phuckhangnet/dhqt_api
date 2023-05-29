using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Users
{
    public class UpdateUserStatusResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public UserDto? RESPONSES { get; set; }
    }
    public class UpdateUserStatusCommand : IRequest<UpdateUserStatusResponse>
    {
        public int? ID { get; set; }

    }
    public class UpdateUserStatusValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserStatusValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID CANNOT BE EMPTY");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateUserStatusHandler : IRequestHandler<UpdateUserStatusCommand, UpdateUserStatusResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateUserStatusHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateUserStatusResponse> Handle(UpdateUserStatusCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.User? _User_to_update = await _dbContext.Users.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_User_to_update != null)
                    {
                        if (_User_to_update.STATUS == 1)
                        {
                            _User_to_update.STATUS = 0;
                            _mapper.Map<UpdateUserStatusCommand, Project.Models.User>(command, _User_to_update);
                            _dbContext.Users.Update(_User_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            dbContextTransaction.Commit();
                            return new UpdateUserStatusResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _mapper.Map<UserDto>(_User_to_update)
                            };
                        }
                        else
                        {
                            _User_to_update.STATUS = 1;
                            _mapper.Map<UpdateUserStatusCommand, Project.Models.User>(command, _User_to_update);
                            _dbContext.Users.Update(_User_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            dbContextTransaction.Commit();
                            return new UpdateUserStatusResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _mapper.Map<UserDto>(_User_to_update)
                            };
                        }

                    }
                    else
                    {
                        return new UpdateUserStatusResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateUserStatusResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
