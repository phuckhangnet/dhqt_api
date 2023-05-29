using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListPosition
{
    public class GetListPositionResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<dynamic>? RESPONSES { get; set; }
    }
    public class GetListPositionCommand : IRequest<GetListPositionResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetListPositionValidator : AbstractValidator<GetListPositionCommand>
    {
        public GetListPositionValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetListPositionHandler : IRequestHandler<GetListPositionCommand, GetListPositionResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetListPositionHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetListPositionResponse> Handle(GetListPositionCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.ListPosition> list_ListPosition_response = Enumerable.Empty<Project.Models.ListPosition>();

                switch (command.Type)
                {
                    case "GET_BY_CODE":
                        list_ListPosition_response = await _dbContext.ListPosition.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                        break;
                    case "GET_ALL":
                        list_ListPosition_response = await _dbContext.ListPosition.ToListAsync(cancellationToken);
                        break;
                }

                return new GetListPositionResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_ListPosition_response
                };
            }
            catch
            {
                return new GetListPositionResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
