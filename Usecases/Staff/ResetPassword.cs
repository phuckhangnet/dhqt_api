using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.UseCases.Tokens;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Users
{
    public class ResetPasswordUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public string? RESPONSES { get; set; }
    }
    public class ResetPasswordUserCommand : IRequest<ResetPasswordUserResponse>
    {
        public string? Username { get; set; }
    }
    public class ResetPasswordUserValidator : AbstractValidator<ResetPasswordUserCommand>
    {
        public ResetPasswordUserValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("USERNAME CANNOT BE EMPTY");
        }
    }
    public class ResetPasswordUserHandler : IRequestHandler<ResetPasswordUserCommand, ResetPasswordUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly ITokenRepository _tokenRepo;

        public ResetPasswordUserHandler(DataContext dbContext, IMapper mapper, ITokenRepository tokenRepo)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _tokenRepo = tokenRepo;
        }
        public async Task<ResetPasswordUserResponse> Handle(ResetPasswordUserCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {

                try
                {
                    GeneralRepository _UserRepo = new GeneralRepository(_dbContext);
                    Project.Models.User? _User_ResetPasswordg = await _dbContext.Users.FirstOrDefaultAsync(x => x.USERNAME == command.Username, cancellationToken);
                    if (_User_ResetPasswordg == null)
                    {
                        return new ResetPasswordUserResponse
                        {
                            MESSAGE = "INCORRECT_USERNAME",
                            STATUSCODE = HttpStatusCode.BadRequest,
                        };
                    }

                    _User_ResetPasswordg.PASSWORDHASH = _UserRepo.HashPassword("123456", out var salt);
                    _User_ResetPasswordg.PASSWORDSALT = Convert.ToHexString(salt);
                    _dbContext.Users.Update(_User_ResetPasswordg);
                    dbContextTransaction.Commit();
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return new ResetPasswordUserResponse
                    {
                        MESSAGE = "RESET_PASSWORD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = "SUCCESS"
                    };

                }
                catch
                {
                    return new ResetPasswordUserResponse
                    {
                        MESSAGE = "RESET_PASSWORD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError,
                        RESPONSES = "FAIL"
                    };
                }
            }

        }
    }
}
