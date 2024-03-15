using MediatR;
using AutoMapper;
using FluentValidation;
using System.Net;
using Project.Data;
using Project.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace Project.UseCases.Menu
{
    public class UpdateMenuResponse
    {
        public string? MESSAGE { get; set; }
        public HttpStatusCode STATUSCODE { get; set; }
        public MenuDto? RESPONSES { get; set; }
        public dynamic? ERROR { get; set; }
    }
    public class UpdateMenuCommand : IRequest<UpdateMenuResponse>
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int MenuLevel { get; set; }
        public string? Parent { get; set; }
        public Int16? Position { get; set; }
        public Byte? Visible { get; set; }
        public Byte? IsActive { get; set; }
        public Byte? IsPage { get; set; }
        public IEnumerable<string?>? Role { get; set; }
        public string? Slug { get; set; }
    }
    public class UpdateMenuValidator : AbstractValidator<UpdateMenuCommand>
    {
        public UpdateMenuValidator()
        {
            RuleFor(x => x.ID).NotNull().NotEmpty().WithMessage("ID không được trống");
        }
    }
    public class UpdateMenuHandler : IRequestHandler<UpdateMenuCommand, UpdateMenuResponse>
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dbContext;

        public UpdateMenuHandler(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateMenuResponse> Handle(UpdateMenuCommand command, CancellationToken cancellationToken)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    GeneralRepository _generalRepo = new GeneralRepository(_dbContext);
                    bool _slug_checked = _dbContext.Menu.Any(u => u.SLUG == command.Slug);
                    Project.Models.Menu _menu_check = _dbContext.Menu.AsNoTracking().FirstOrDefault(x => x.SLUG == command.Slug);

                    List<string> _existed_prop = new List<string> { _slug_checked ? "SLUG_ALREADY_EXISTS" : string.Empty };
                    _existed_prop.RemoveAll(s => s == string.Empty);
                    if (_existed_prop.Count() > 0 && _menu_check.ID != command.ID)
                    {
                        return new UpdateMenuResponse
                        {
                            MESSAGE = "UPDATE_FAIL",
                            STATUSCODE = HttpStatusCode.InternalServerError,
                            ERROR = _existed_prop
                        };
                    }
                    else
                    {
                        Project.Models.Menu? _Menu_to_update = await _dbContext.Menu.FirstOrDefaultAsync(x => x.ID == command.ID, cancellationToken);
                        if (_Menu_to_update != null)
                        {
                            _mapper.Map<UpdateMenuCommand, Project.Models.Menu>(command, _Menu_to_update);
                            _dbContext.Menu.Update(_Menu_to_update);
                            await _dbContext.SaveChangesAsync(cancellationToken);

                            _dbContext.Role_Menu.RemoveRange(_dbContext.Role_Menu.Where(x => x.MENUID == command.ID));
                            _dbContext.SaveChanges();

                            Project.Models.Role_Menu _Role_Menu_to_add = new Project.Models.Role_Menu();
                            if (command.Role != null)
                            {
                                foreach (var _role in command.Role)
                                {
                                    _Role_Menu_to_add.MENUID = command.ID;
                                    _Role_Menu_to_add.ROLECODE = _role;
                                    _dbContext.Add(_Role_Menu_to_add);
                                    _dbContext.SaveChanges();
                                }
                            }

                            dbContextTransaction.Commit();
                            return new UpdateMenuResponse
                            {
                                MESSAGE = "UPDATE_SUCCESSFUL",
                                STATUSCODE = HttpStatusCode.OK,
                                RESPONSES = _mapper.Map<MenuDto>(_Menu_to_update)
                            };
                        }
                        else
                        {
                            return new UpdateMenuResponse
                            {
                                MESSAGE = "UPDATE_FAIL",
                                STATUSCODE = HttpStatusCode.BadRequest
                            };
                        }
                    }
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return new UpdateMenuResponse
                    {
                        MESSAGE = "UPDATE_FAIL",
                        STATUSCODE = HttpStatusCode.InternalServerError
                    };
                }
            }
        }
    }
}