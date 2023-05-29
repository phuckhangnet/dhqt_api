using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
namespace Project.UseCases.Rule
{
    public class AddRuleResponse 
    {
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public RuleDto? RESPONSES {get;set;} 
        public dynamic? ERROR {get;set;}
    }
    public class AddRuleCommand : IRequest<AddRuleResponse>
    {
        public string? Code {get;set;}
        public string? Description {get;set;}
    }
    public class AddRuleValidator : AbstractValidator<AddRuleCommand>
    {
        public AddRuleValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("Mã quyền không được trống");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Mô tả không được trống");
        }
    }
    public class AddRuleHandler : IRequestHandler<AddRuleCommand, AddRuleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly RuleRepository _RuleRepo;

        public AddRuleHandler(DataContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_RuleRepo = RuleRepo;
        }
        public async Task<AddRuleResponse> Handle(AddRuleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);

                    Project.Models.Rule _Rule_to_add = _mapper.Map<Project.Models.Rule>(command);
                    await _dbContext.AddAsync(_Rule_to_add, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    dbContextTransaction.Commit();
                    return new AddRuleResponse {
                        MESSAGE = "Tạo Rule thành công!",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<RuleDto>(_Rule_to_add)
                    };
                }
                catch {
                    dbContextTransaction.Rollback();
                    return new AddRuleResponse {
                        MESSAGE = "Tạo Rule thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
