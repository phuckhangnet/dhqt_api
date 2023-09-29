using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.ListLanguage
{
    public class UpdateListLanguageResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
    }
    public class UpdateListLanguageCommand : IRequest<UpdateListLanguageResponse>
    {
        public string? Code { get; set; }
        public string? Lang { get; set; }
        public string? Text { get; set; }
    }
    public class UpdateListLanguageValidator : AbstractValidator<UpdateListLanguageCommand>
    {
        public UpdateListLanguageValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("ID không được trống");
            // RuleFor(x => x.Identify).NotNull().NotEmpty().WithMessage("CMND không được trống");
            // RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email không được trống");
            // RuleFor(x => x.Phone).NotNull().NotEmpty().WithMessage("SĐT không được trống");
            // RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password không được trống");
        }
    }
    public class UpdateListLanguageHandler : IRequestHandler<UpdateListLanguageCommand, UpdateListLanguageResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateListLanguageHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateListLanguageResponse> Handle(UpdateListLanguageCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    Project.Models.ListLanguage? _ListLanguage_to_update = await _dbContext.ListLanguage.FirstOrDefaultAsync(x => x.CODE == command.Code && x.LANG == command.Lang, cancellationToken);
                    if (_ListLanguage_to_update != null)
                    {
                        _mapper.Map<UpdateListLanguageCommand, Project.Models.ListLanguage>(command, _ListLanguage_to_update);
                        _dbContext.ListLanguage.Update(_ListLanguage_to_update);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        dbContextTransaction.Commit();
                    }
                    return new UpdateListLanguageResponse
                    {
                        MESSAGE = "UPDATE_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _ListLanguage_to_update
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateListLanguageResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
