using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Rule
{
    public class GetRuleResponse 
    {
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public IEnumerable<RuleDto>? RESPONSES {get;set;} 
    }
    public class GetRuleCommand : IRequest<GetRuleResponse>
    {
        public string? Type {get;set;}
        public IEnumerable<string> Data {get;set;} = Enumerable.Empty<string>();
    }
    public class GetRuleValidator : AbstractValidator<GetRuleCommand>
    {
        public GetRuleValidator()
        {
            RuleFor(x => x.Type).NotNull().NotEmpty().WithMessage("Loại truy vấn không được trống");
            RuleFor(x => x.Data).NotNull().NotEmpty().WithMessage("Thông tin truy vấn không được trống");
        }
    }
    public class GetRuleHandler : IRequestHandler<GetRuleCommand, GetRuleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public GetRuleHandler(DataContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<GetRuleResponse> Handle(GetRuleCommand command, CancellationToken cancellationToken)
        {
            
                try {
                    IEnumerable<Project.Models.Rule> list_Rule_response = Enumerable.Empty<Project.Models.Rule>();

                    switch (command.Type)
                    {
                        case "GET_BY_CODE":
                            list_Rule_response = await _dbContext.Rule.Where(x => command.Data.Contains(x.CODE)).ToListAsync(cancellationToken);
                            break;
                        case "GET_BY_ID" :
                            list_Rule_response = await _dbContext.Rule.Where(x => command.Data.Contains(x.ID.ToString())).ToListAsync(cancellationToken);
                            break;
                        default:
                            list_Rule_response = await _dbContext.Rule.ToListAsync(cancellationToken);
                            break;
                    }
                    
                    return new GetRuleResponse {
                        MESSAGE = "Truy vấn thành công!",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<IEnumerable<RuleDto>>(list_Rule_response)
                    };
                }
                catch {
                    return new GetRuleResponse {
                        MESSAGE = "Thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            
        }
    }
}
