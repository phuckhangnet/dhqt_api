using MediatR;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Project.UseCases.Tokens;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Users
{
    public class LoginUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public UserLoginDto? RESPONSES { get; set; }
        public dynamic? menuList { get; set; }
        public dynamic? roleList { get; set; }
    }
    public class LoginUserCommand : IRequest<LoginUserResponse>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("USERNAME CANNOT BE EMPTY");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("PASSWORD CANNOT BE EMPTY");
        }
    }
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        private readonly ITokenRepository _tokenRepo;

        public LoginUserHandler(DataContext dbContext, IMapper mapper, ITokenRepository tokenRepo)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _tokenRepo = tokenRepo;
        }
        public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {

            try
            {
                GeneralRepository _UserRepo = new GeneralRepository(_dbContext);
                Project.Models.User? _User_loging = await _dbContext.Users.FirstOrDefaultAsync(x => x.USERNAME == command.Username, cancellationToken);
                if (_User_loging == null)
                {
                    return new LoginUserResponse
                    {
                        MESSAGE = "INCORRECT_USER_NAME",
                        STATUSCODE = HttpStatusCode.OK,
                    };
                }
                else if (!_UserRepo.ComparePassword(command.Password, _User_loging.PASSWORDHASH, _User_loging.PASSWORDSALT))
                {
                    return new LoginUserResponse
                    {
                        MESSAGE = "INCORRECT_PASSWORD",
                        STATUSCODE = HttpStatusCode.OK,
                    };
                }
                UserLoginDto _User_login_dto = _mapper.Map<UserLoginDto>(_User_loging);
                var _User_login_dto_detail = _dbContext.User_Detail.Where(x => x.USERID == _User_login_dto.ID).FirstOrDefault();
                var user_claims = new Claim[] { };
                user_claims = new[] {
                        new Claim("ID", _User_login_dto.ID.ToString()),
                        new Claim("Username", _User_login_dto.USERNAME),
                        new Claim("Name", _User_login_dto_detail.NAME),
                        //new Claim("Code", _User_login_dto.CODE)
                    };
                _User_login_dto.TOKEN = _tokenRepo.BuildToken(user_claims);
                _User_login_dto.TOKENALIVETIME = _tokenRepo.GetTokenAliveTime();
                var iduser = _User_login_dto.ID.ToString();
                var _iduser = Int32.Parse(iduser);
                var user = _dbContext.Users.Where(x => x.ID == _iduser).ToList();
                var userRole = user.First().ROLE;
                dynamic result;
                if (userRole.IndexOf(",") > -1)
                {
                    var role = userRole.Split(",")[0];
                    result = await _dbContext.Role_Menu.Where(x => x.ROLECODE == role).Select(x => x.MENUID).ToListAsync();
                }
                else
                {
                    result = await _dbContext.Role_Menu.Where(x => x.ROLECODE == userRole).Select(x => x.MENUID).ToListAsync();
                }
                var roleList = _dbContext.Role.Where(x => (userRole + ",").Contains(x.CODE)).ToList();

                return new LoginUserResponse
                {
                    MESSAGE = "LOGIN_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = _User_login_dto,
                    menuList = result,
                    roleList = roleList
                };
            }
            catch
            {
                return new LoginUserResponse
                {
                    MESSAGE = "LOGIN_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
