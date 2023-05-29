using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Rule
{
    public class UpdateRuleResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public RuleDto? RESPONSES { get; set; }
    }
    public class UpdateRuleCommand : IRequest<UpdateRuleResponse>
    {
        public int? ID { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    public class UpdateRuleValidator : AbstractValidator<UpdateRuleCommand>
    {
        public UpdateRuleValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID không được trống");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateRuleHandler : IRequestHandler<UpdateRuleCommand, UpdateRuleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateRuleHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateRuleResponse> Handle(UpdateRuleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Rule? _Rule_to_update = await _dbContext.Rule.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Rule_to_update != null)
                    {
                        _mapper.Map<UpdateRuleCommand, Project.Models.Rule>(command, _Rule_to_update);
                        _dbContext.Rule.Update(_Rule_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new UpdateRuleResponse
                        {
                            MESSAGE = "Cập nhật thành công!",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<RuleDto>(_Rule_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateRuleResponse
                        {
                            MESSAGE = "Cập nhật thất bại!",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateRuleResponse
                    {
                        MESSAGE = "Cập nhật thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
