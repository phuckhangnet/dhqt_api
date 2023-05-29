using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Hastag
{
    public class GetHastagResponse 
    {
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public IEnumerable<HastagDto>? RESPONSES {get;set;} 
    }
    public class GetHastagCommand : IRequest<GetHastagResponse>
    {
        public string? Type {get;set;}
        public IEnumerable<string> Data {get;set;} = Enumerable.Empty<string>();
    }
    public class GetHastagValidator : AbstractValidator<GetHastagCommand>
    {
        public GetHastagValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetHastagHandler : IRequestHandler<GetHastagCommand, GetHastagResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetHastagHandler(DataContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetHastagResponse> Handle(GetHastagCommand command, CancellationToken cancellationToken)
        {
            
                try {
                    IEnumerable<Project.Models.Hastag> list_Hastag_response = Enumerable.Empty<Project.Models.Hastag>();

                    switch (command.Type)
                    {
                        case "GET_BY_CODE":
                            list_Hastag_response = await _dbContext.Hastag.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                            break;
                        case "GET_BY_ID" :
                            list_Hastag_response = await _dbContext.Hastag.Where(x => command.Data.Contains(x.ID.ToString())).ToListAsync(cancellationToken);
                            break;
                        default:
                            list_Hastag_response = await _dbContext.Hastag.ToListAsync(cancellationToken);
                            break;
                    }
                    
                    return new GetHastagResponse {
                        MESSAGE = "Truy vấn thành công!",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<IEnumerable<HastagDto>>(list_Hastag_response)
                    };
                }
                catch {
                    return new GetHastagResponse {
                        MESSAGE = "Thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            
        }
    }
}
