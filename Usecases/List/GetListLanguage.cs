using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListLanguage
{
    public class GetListLanguageResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public IEnumerable<dynamic>? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class GetListLanguageCommand : IRequest<GetListLanguageResponse>
    {
        public string? Type { get; set; }
        public IEnumerable<string> Data { get; set; } = Enumerable.Empty<string>();
    }
    public class GetListLanguageValidator : AbstractValidator<GetListLanguageCommand>
    {
        public GetListLanguageValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetListLanguageHandler : IRequestHandler<GetListLanguageCommand, GetListLanguageResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetListLanguageHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetListLanguageResponse> Handle(GetListLanguageCommand command, CancellationToken cancellationToken)
        {

            try
            {
                IEnumerable<Project.Models.ListLanguage> list_ListLanguage_response = Enumerable.Empty<Project.Models.ListLanguage>();

                switch (command.Type)
                {
                    case "GET_BY_CODE":
                        list_ListLanguage_response = await _dbContext.ListLanguage.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                        break;
                    case "GET_ALL":
                        list_ListLanguage_response = await _dbContext.ListLanguage.ToListAsync(cancellationToken);
                        break;
                }

                return new GetListLanguageResponse
                {
                    MESSAGE = "GET_SUCCESSFUL",
                    STATUSCODE = HttpStatusCode.OK,
                    RESPONSES = list_ListLanguage_response
                };
            }
            catch (Exception ex)
            {
                return new GetListLanguageResponse
                {
                    MESSAGE = "GET_FAIL",
                    STATUSCODE = HttpStatusCode.InternalServerError,
                    ERROR = ex.ToString()
                };
            }

        }
    }
}
