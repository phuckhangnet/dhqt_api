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
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public dynamic? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class AddMenuCommand : IRequest<AddMenuResponse>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int MenuLevel { get; set; }
        public string? Parent { get; set; }
        public Int16? Position { get; set; }
        public Byte? Visible { get; set; }
        public Byte? IsActive { get; set; }
        public Byte? IsPage { get; set; }
        public IEnumerable<string?>? Role { get; set; }
    }
    public class AddMenuValidator : AbstractValidator<AddMenuCommand>
    {
        public AddMenuValidator()
        {

        }
    }
    public class AddMenuHandler : IRequestHandler<AddMenuCommand, AddMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;
        //private readonly MenuRepository _MenuRepo;

        public AddMenuHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            //_MenuRepo = MenuRepo;
        }
        public async Task<AddMenuResponse> Handle(AddMenuCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);

                    Project.Models.Menu _Menu_to_add = _mapper.Map<Project.Models.Menu>(command);
                    Project.Models.Role_Menu _Role_Menu = _mapper.Map<Project.Models.Role_Menu>(command);
                    await _dbContext.AddAsync(_Menu_to_add, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    _dbContext.Entry(_Menu_to_add).GetDatabaseValues();

                    _Role_Menu.MENUID = _Menu_to_add.ID;
                    foreach (string _role in command.Role)
                    {
                        _Role_Menu.MENUID = _Menu_to_add.ID;
                        _Role_Menu.ROLECODE = _role;
                        _dbContext.Add(_Role_Menu);
                        _dbContext.SaveChanges();
                    }

                    dbContextTransaction.Commit();

                    return new AddMenuResponse
                    {
                        MESSAGE = "ADD_SUCCESSFUL",
                        STATUSCODE = HttpStatusCode.OK,
                        RESPONSES = _Menu_to_add
                    };
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new AddMenuResponse
                    {
                        MESSAGE = "ADD_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}
