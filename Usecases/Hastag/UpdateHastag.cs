using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Hastag
{
    public class UpdateHastagResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public HastagDto? RESPONSES { get; set; }
    }
    public class UpdateHastagCommand : IRequest<UpdateHastagResponse>
    {
        public int? ID { get; set; }
        public string? Code { get; set; }
    }
    public class UpdateHastagValidator : AbstractValidator<UpdateHastagCommand>
    {
        public UpdateHastagValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID không được trống");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateHastagHandler : IRequestHandler<UpdateHastagCommand, UpdateHastagResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateHastagHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateHastagResponse> Handle(UpdateHastagCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Project.Models.Hastag? _Hastag_to_update = await _dbContext.Hastag.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                    if (_Hastag_to_update != null)
                    {
                        _mapper.Map<UpdateHastagCommand, Project.Models.Hastag>(command, _Hastag_to_update);
                        _dbContext.Hastag.Update(_Hastag_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                        return new UpdateHastagResponse
                        {
                            MESSAGE = "Cập nhật thành công!",
                            STATUSCODE = HttpStatusCode.OK,
                            RESPONSES = _mapper.Map<HastagDto>(_Hastag_to_update)
                        };
                    }
                    else
                    {
                        return new UpdateHastagResponse
                        {
                            MESSAGE = "Cập nhật thất bại!",
                            STATUSCODE = HttpStatusCode.BadRequest
                        };
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateHastagResponse
                    {
                        MESSAGE = "Cập nhật thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
