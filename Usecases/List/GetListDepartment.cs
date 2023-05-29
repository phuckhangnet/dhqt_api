using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListDepartment
{
    public class GetListDepartmentResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<dynamic>? RESPONSES { get; set; }
    }
    public class GetListDepartmentCommand : IRequest<GetListDepartmentResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetListDepartmentValidator : AbstractValidator<GetListDepartmentCommand>
    {
        public GetListDepartmentValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetListDepartmentHandler : IRequestHandler<GetListDepartmentCommand, GetListDepartmentResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetListDepartmentHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetListDepartmentResponse> Handle(GetListDepartmentCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.ListDepartment> list_ListDepartment_response = Enumerable.Empty<Project.Models.ListDepartment>();

                switch (command.Type)
                {
                    case "GET_BY_CODE":
                        list_ListDepartment_response = await _dbContext.ListDepartment.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                        break;
                    case "GET_ALL":
                        list_ListDepartment_response = await _dbContext.ListDepartment.ToListAsync(cancellationToken);
                        break;
                }

                return new GetListDepartmentResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_ListDepartment_response
                };
            }
            catch
            {
                return new GetListDepartmentResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
