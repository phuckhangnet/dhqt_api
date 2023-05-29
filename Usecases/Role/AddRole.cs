using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
namespace Project.UseCases.Role
{
    public class AddRoleResponse 
    {
        public string? MESSAGE {get;set;}
        public HttpStatusCode STATUSCODE {get;set;}
        public RoleDto? RESPONSES {get;set;} 
        public dynamic? ERROR {get;set;}
    }
    public class AddRoleCommand : IRequest<AddRoleResponse>
    {
        public string? Code {get;set;}
        public string? Rule_List {get;set;}
    }
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {
        public AddRoleValidator()
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("Mã không được trống");
            RuleFor(x => x.Rule_List).NotNull().NotEmpty().WithMessage("Danh sách quyền không được trống");
        }
    }
    public class AddRoleHandler : IRequestHandler<AddRoleCommand, AddRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly RoleRepository _RoleRepo;

        public AddRoleHandler(DataContext dbContext,IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_RoleRepo = RoleRepo;
        }
        public async Task<AddRoleResponse> Handle(AddRoleCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);

                    Project.Models.Role _Role_to_add = _mapper.Map<Project.Models.Role>(command);
                    await _dbContext.AddAsync(_Role_to_add, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    dbContextTransaction.Commit();
                    return new AddRoleResponse {
                        MESSAGE = "Tạo Role thành công!",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _mapper.Map<RoleDto>(_Role_to_add)
                    };
                }
                catch {
                    dbContextTransaction.Rollback();
                    return new AddRoleResponse {
                        MESSAGE = "Tạo Role thất bại!",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
