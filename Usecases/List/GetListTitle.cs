using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListTitle
{
    public class GetListTitleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<dynamic>? RESPONSES { get; set; }
    }
    public class GetListTitleCommand : IRequest<GetListTitleResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetListTitleValidator : AbstractValidator<GetListTitleCommand>
    {
        public GetListTitleValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetListTitleHandler : IRequestHandler<GetListTitleCommand, GetListTitleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetListTitleHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetListTitleResponse> Handle(GetListTitleCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.ListTitle> list_ListTitle_response = Enumerable.Empty<Project.Models.ListTitle>();

                switch (command.Type)
                {
                    case "GET_BY_CODE":
                        list_ListTitle_response = await _dbContext.ListTitle.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                        break;
                    case "GET_ALL":
                        list_ListTitle_response = await _dbContext.ListTitle.ToListAsync(cancellationToken);
                        break;
                }

                return new GetListTitleResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_ListTitle_response
                };
            }
            catch
            {
                return new GetListTitleResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
