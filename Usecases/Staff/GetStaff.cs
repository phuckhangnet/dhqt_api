using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Users
{
    public class GetUserResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
        public dynamic? ROLE { get; set; }
    }
    public class GetUserCommand : IRequest<GetUserResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetUserValidator : AbstractValidator<GetUserCommand>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("INCORRECT QUERY TYPE");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("INCORRECT QUERY INFORMATION");
        }
    }
    public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetUserHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetUserResponse> Handle(GetUserCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.User> list_User_response = Enumerable.Empty<Project.Models.User>();
                if (command.Type == null)
                {
                    return new GetUserResponse
                    {
                        MESSAGE = "MISSING_PARAMETER_TYPE",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
                else if (command.Type == "GET_ALL")
                {

                    var title = _dbContext.User_List.Where(x => x.TABLELIST == "TITLE").Join(_dbContext.ListTitle, a => a.LISTCODE, b => b.CODE, (a, b) => new { a, b })
                                .GroupBy(g => new { g.a.USERID })
                                .Select(g => new
                                {
                                    g.Key.USERID,
                                    CODE = string.Join(", ", g.Select(x => x.b.CODE)),
                                    DESCRIPTION = string.Join(", ", g.Select(x => x.b.DESCRIPTION))
                                });
                    var position = _dbContext.User_List.Where(x => x.TABLELIST == "POSITION").Join(_dbContext.ListPosition, a => a.LISTCODE, b => b.CODE, (a, b) => new { a, b })
                                .GroupBy(g => new { g.a.USERID })
                                .Select(g => new
                                {
                                    g.Key.USERID,
                                    CODE = string.Join(", ", g.Select(x => x.b.CODE)),
                                    DESCRIPTION = string.Join(", ", g.Select(x => x.b.DESCRIPTION))
                                });
                    var department = _dbContext.User_List.Where(x => x.TABLELIST == "DEPARTMENT").Join(_dbContext.ListDepartment, a => a.LISTCODE, b => b.CODE, (a, b) => new { a, b })
                                .GroupBy(g => new { g.a.USERID })
                                .Select(g => new
                                {
                                    g.Key.USERID,
                                    CODE = string.Join(", ", g.Select(x => x.b.CODE)),
                                    DESCRIPTION = string.Join(", ", g.Select(x => x.b.DESCRIPTION))
                                });
                    var user = _mapper.Map<IEnumerable<UserDto>>(_dbContext.Users).Join(_dbContext.User_Detail, USER => USER.ID, USERDETAIL => USERDETAIL.USERID
                                , (USER, USERDETAIL) => new
                                {
                                    USER,
                                    USERDETAIL,
                                    TITLES = title.Where(x => x.USERID == USER.ID).Select(x => new { CODE = x.CODE, DESCRIPTION = x.DESCRIPTION }).FirstOrDefault(),
                                    POSITIONS = position.Where(x => x.USERID == USER.ID).Select(x => new { CODE = x.CODE, DESCRIPTION = x.DESCRIPTION }).FirstOrDefault(),
                                    DEPARTMENTS = department.Where(x => x.USERID == USER.ID).Select(x => new { CODE = x.CODE, DESCRIPTION = x.DESCRIPTION }).FirstOrDefault(),
                                    ROLESDESCRIPTION = string.Join(", ", _dbContext.Role.Where(x => (USER.ROLE + ",").Contains(x.CODE)).Select(x => x.DESCRIPTION.ToString()).ToList()),
                                });

                    return new GetUserResponse
                    {
                        MESSAGE = "GET_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = user
                    };
                }
                else
                {
                    return new GetUserResponse
                    {
                        MESSAGE = "GET_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
            catch
            {
                return new GetUserResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
