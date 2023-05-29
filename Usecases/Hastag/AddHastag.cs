using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
namespace Project.UseCases.Hastag
{
    public class AddHastagResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public HastagDto? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddHastagCommand : IRequest<AddHastagResponse>
    {
        public string? Code { get; set; }
    }
    public class AddHastagValidator : AbstractValidator<AddHastagCommand>
    {
        public AddHastagValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("CODE CANNOT BE EMPTY");
        }
    }
    public class AddHastagHandler : IRequestHandler<AddHastagCommand, AddHastagResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly HastagRepository _HastagRepo;

        public AddHastagHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_HastagRepo = HastagRepo;
        }
        public async Task<AddHastagResponse> Handle(AddHastagCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);

                    Project.Models.Hastag _Hastag_to_add = _mapper.Map<Project.Models.Hastag>(command);
                    await _dbContext.AddAsync(_Hastag_to_add, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    dbContextTransaction.Commit();
                    return new AddHastagResponse
                    {
                        MESSAGE = "ADD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<HastagDto>(_Hastag_to_add)
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddHastagResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
