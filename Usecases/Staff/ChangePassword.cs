using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.UseCases.Tokens;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Users
{
    public class ChangePasswordUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public string? RESPONSES { get; set; }
    }
    public class ChangePasswordUserCommand : IRequest<ChangePasswordUserResponse>
    {
        public string? Username { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
    public class ChangePasswordUserValidator : AbstractValidator<ChangePasswordUserCommand>
    {
        public ChangePasswordUserValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("USERNAME CANNOT BE EMPTY");
            RuleFor(x => x.OldPassword).NotNull().NotEmpty().WithMessage("OLD PASSWORD CANNOT BE EMPTY");
            RuleFor(x => x.NewPassword).NotNull().NotEmpty().WithMessage("NEW PASSWORD CANNOT BE EMPTY");
        }
    }
    public class ChangePasswordUserHandler : IRequestHandler<ChangePasswordUserCommand, ChangePasswordUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly ITokenRepository _tokenRepo;

        public ChangePasswordUserHandler(DataContext dbContext, IMapper mapper, ITokenRepository tokenRepo)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _tokenRepo = tokenRepo;
        }
        public async Task<ChangePasswordUserResponse> Handle(ChangePasswordUserCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    GeneralRepository _UserRepo = new GeneralRepository(_dbContext);
                    Project.Models.User? _User_ChangePasswordg = await _dbContext.Users.FirstOrDefaultAsync(x => x.USERNAME == command.Username, cancellationToken);
                    if (_User_ChangePasswordg == null)
                    {
                        return new ChangePasswordUserResponse
                        {
                            MESSAGE = "INCORRECT_USERNAME",
                            STATUSCODE = HttpStatusCode.BadRequest,
                        };
                    }
                    bool Compare_password = _UserRepo.ComparePassword(command.OldPassword, _User_ChangePasswordg.PASSWORDHASH, _User_ChangePasswordg.PASSWORDSALT);
                    if (!Compare_password)
                    {
                        return new ChangePasswordUserResponse
                        {
                            MESSAGE = "INCORRECT_OLD_PASSWORD",
                            STATUSCODE = HttpStatusCode.BadRequest,
                        };
                    }
                    else
                    {
                        _User_ChangePasswordg.PASSWORD = command.NewPassword;
                        _User_ChangePasswordg.PASSWORDHASH = _UserRepo.HashPassword(command.NewPassword, out var salt);
                        _User_ChangePasswordg.PASSWORDSALT = Convert.ToHexString(salt);
                        _dbContext.Users.Update(_User_ChangePasswordg);
                        dbContextTransaction.Commit();
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        return new ChangePasswordUserResponse
                        {
                            MESSAGE = "CHANGE_PASSWORD_SUCCESSFUL",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = "SUCCESS"
                        };
                    }
                }
                catch
                {
                    return new ChangePasswordUserResponse
                    {
                        MESSAGE = "CHANGE_PASSWORD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError,
                        RESPONSES = "FAIL"
                    };
                }
            }

        }
    }
}
