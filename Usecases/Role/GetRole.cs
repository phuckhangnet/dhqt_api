using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Role
{
    public class GetRoleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<RoleDto>? RESPONSES { get; set; }
    }
    public class GetRoleCommand : IRequest<GetRoleResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetRoleValidator : AbstractValidator<GetRoleCommand>
    {
        public GetRoleValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetRoleHandler : IRequestHandler<GetRoleCommand, GetRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetRoleHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetRoleResponse> Handle(GetRoleCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.Role> list_Role_response = Enumerable.Empty<Project.Models.Role>();

                switch (command.Type)
                {
                    case "GET_BY_CODE":
                        list_Role_response = await _dbContext.Role.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                        break;
                    default:
                        list_Role_response = await _dbContext.Role.ToListAsync(cancellationToken);
                        break;
                }

                return new GetRoleResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = _mapper.Map<IEnumerable<RoleDto>>(list_Role_response)
                };
            }
            catch
            {
                return new GetRoleResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
