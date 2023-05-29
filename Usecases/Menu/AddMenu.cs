using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
namespace Project.UseCases.Menu
{
    public class AddMenuResponse 
    {
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public MenuDto? RESPONSES {get;set;} 
        public dynamic? ERROR {get;set;}
    }
    public class AddMenuCommand : IRequest<AddMenuResponse>
    {
        public string? Description {get;set;}
        public string? Hastag {get;set;}
        public int Menu_Level {get;set;}
    }
    public class AddMenuValidator : AbstractValidator<AddMenuCommand>
    {
        public AddMenuValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Mô tả không được trống");
            RuleFor(x => x.Hastag).NotNull().NotEmpty().WithMessage("Hastag không được trống");
            RuleFor(x => x.Menu_Level).NotNull().NotEmpty().WithMessage("Cấp độ không được trống");
        }
    }
    public class AddMenuHandler : IRequestHandler<AddMenuCommand, AddMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly MenuRepository _MenuRepo;

        public AddMenuHandler(DataContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_MenuRepo = MenuRepo;
        }
        public async Task<AddMenuResponse> Handle(AddMenuCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);

                    Project.Models.Menu _Menu_to_add = _mapper.Map<Project.Models.Menu>(command);
                    await _dbContext.AddAsync(_Menu_to_add, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    dbContextTransaction.Commit();
                    return new AddMenuResponse {
                        MESSAGE = "Tạo menu thành công!",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<MenuDto>(_Menu_to_add)
                    };
                }
                catch {
                    dbContextTransaction.Rollback();
                    return new AddMenuResponse {
                        MESSAGE = "Tạo menu thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
